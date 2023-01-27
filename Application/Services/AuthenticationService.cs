using Application.Contracts;
using Application.DataTransferObjects;
using Application.Helpers;
using AutoMapper;
using Domain.ConfigurationModels;
using Domain.Entities;
using Domain.Entities.Identities;
using Domain.Enums;
using Domain.Exceptions;
using Infrastructure.Contracts;
using Infrastructure.Utils.Email;
using Infrastructure.Utils.Logger;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Application.Services;

public class AuthenticationService : IAuthenticationService
{
    private readonly IRepositoryManager _repository;
    private readonly IConfiguration _configuration;
    private readonly UserManager<User> _userManager;
    private readonly ILoggerManager _logger;
    private readonly IMapper _mapper;
    private readonly IEmailManager _emailManager;
    private readonly JwtConfiguration _jwtConfiguration;

    public AuthenticationService(IRepositoryManager repository, UserManager<User> userManager,
        IEmailManager emailManager, IMapper mapper, ILoggerManager logger, IConfiguration configuration)
    {
        _configuration = configuration;
        _userManager = userManager;
        _repository = repository;
        _logger = logger;
        _mapper = mapper;
        _emailManager = emailManager;
        _jwtConfiguration = new JwtConfiguration();
        _configuration.Bind(_jwtConfiguration.Section, _jwtConfiguration);
    }

    public async Task<SuccessResponse<AuthDto>> Login(UserLoginDTO model)
    {
        var email = model.Email.Trim().ToLower();

        var user = await _userManager.FindByEmailAsync(email);
        if (user is null)
            throw new RestException(HttpStatusCode.NotFound, "Wrong Email");

        var authenticated = await ValidateUser(user, model.Password);
        if (!authenticated)
            throw new RestException(HttpStatusCode.Unauthorized, "Wrong Email or Password");

        //check if user is disabled or active or pending
        CheckUserStatus(user);

        user.LastLogin = DateTime.UtcNow;
        await _userManager.UpdateAsync(user);

        var userActivity = new UserActivity()
        {
            EventType = "User Login",
            UserId = user.Id,
            ObjectClass = "USER",
            Details = "Logged in",
            ObjectId = user.Id
        };

        await _repository.UserActivity.AddAsync(userActivity);
        await _repository.SaveChangesAsync();

        var token = await CreateToken(user, true);
        return new SuccessResponse<AuthDto>
        {
            Data = token
        };
    }

    public async Task<SuccessResponse<AuthDto>> RefreshToken(RefreshTokenDTO model)
    {
        var principal = GetPrincipalFromExpiredToken(model.RefreshToken);
        if (principal.Identity != null)
        {
            var user = await _userManager.FindByNameAsync(principal.Identity.Name);
            if (user == null || user.RefreshToken != model.RefreshToken || user.RefreshTokenExpiryTime < DateTime.Now)
                throw new RefreshTokenBadRequest();
            return new SuccessResponse<AuthDto>
            {
                Data = await CreateToken(user, populateExp: false)
            };
        }
        throw new RestException(HttpStatusCode.Unauthorized, "This specified token has expired, please login");
    }


    public async Task<TokenResponse<object>> VerifyToken(string token)
    {
        var tokenEntity = await _repository.Token.FirstOrDefaultAsync(x => x.Value == token);
        if (tokenEntity == null)
            return new TokenResponse<object>
            {
                Message = "Invalid Token"
            };

        if (DateTime.Now >= tokenEntity.ExpiresAt)
        {
            _repository.Token.Remove(tokenEntity);
            await _repository.SaveChangesAsync();
            return new TokenResponse<object>
            {
                Message = "Invalid Token"
            };
        }

        var user = await _repository.User.FirstOrDefaultAsync(x => x.Id == tokenEntity.UserId);
        if (user == null)
            throw new RestException(HttpStatusCode.BadRequest, "Invalid User");

        return new TokenResponse<object>
        {
            Message = "Valid Token",
            Data = user,
            IsValid = true
        };
    }
    public async Task<SuccessResponse<object>> SetPassword(SetPasswordDTO model)
    {
        var tokenEntity = await _repository.Token.FirstOrDefaultAsync(x => x.Value == model.Token);

        ValidateUserToken(tokenEntity);

        var user = await _repository.User.GetByIdAsync(tokenEntity.UserId);
            throw new RestException(HttpStatusCode.NotFound, "The User cannot be found.");

        user.PasswordHash = _userManager.PasswordHasher.HashPassword(user, model.Password);
        user.Verified = true;
        user.EmailConfirmed = true;
        user.IsActive = true;
        user.UpdatedAt = DateTime.UtcNow;
        user.IsActive = true;

        if (tokenEntity.TokenType == ETokenType.InviteUser.ToString())
        {
            user.IsActive = true;
            user.Status = EUserStatus.Active.ToString();
            user.EmailConfirmed = true;
            user.Verified = true;
        }
        _repository.User.Update(user);


        var userActivity = new UserActivity
        {
            EventType = "Set Password",
            UserId = user.Id,
            Details = "Set password",
            ObjectClass = "USER",
            ObjectId = user.Id
        };

        await _repository.UserActivity.AddAsync(userActivity);
        await _repository.SaveChangesAsync();

        return new SuccessResponse<object>
        {
            Message = "Password set successfully"
        };
    }

    private async Task<bool> ValidateUser(User user, string password)
    {
        var result = (user != null && await _userManager.CheckPasswordAsync(user, password));
        if (!result)
            _logger.LogWarn($"{nameof(ValidateUser)}: Authentication failed, wrong email or password");

        if (user != null && !user.Verified)
        {
            _logger.LogWarn($"{nameof(ValidateUser)}: Authentication failed, User is not verified");
            return false;
        }
        return result;
    }
    private async Task<AuthDto> CreateToken(User user, bool populateExp)
    {
        var signingCredentials = GetSigningCredentials();
        var claims = await GetClaims(user);
        var tokenOptions = GenerateTokenOptions(signingCredentials, claims);
        var refreshToken = GenerateRefreshToken();
        if (populateExp)
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
        await _userManager.UpdateAsync(user);
        var accessToken = new JwtSecurityTokenHandler().WriteToken(tokenOptions);
        return new AuthDto
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken,
            ExpiresIn = user.RefreshTokenExpiryTime
        };

    }
    private string GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
    }
    private ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
    {
        //var jwtSettins = _configuration.GetSection("JwtSettings");
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = false,
            ValidateIssuer = false,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtConfiguration.Secret)),
            ValidateLifetime = false,
            ValidAudience = _jwtConfiguration.ValidAudience,
            ValidIssuer = _jwtConfiguration.ValidIssuer
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        SecurityToken securityToken;
        var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);
        var jwtSecurityToken = securityToken as JwtSecurityToken;
        if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
               StringComparison.InvariantCultureIgnoreCase))
            throw new SecurityTokenException("Invalid token");

        return principal;
    }
    private SigningCredentials GetSigningCredentials()
    {
        var jwtSecret = _configuration.GetSection("JwtSettings")["secret"];
        var key = Encoding.UTF8.GetBytes(jwtSecret);
        var secret = new SymmetricSecurityKey(key);
        return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
    }

    private async Task<List<Claim>> GetClaims(User user)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user!.Email),
            new Claim("Email", user.Email),
            new Claim("UserId", user.Id.ToString()),
            new Claim("FirstName", user.FirstName),
            new Claim("LastName", user.LastName),
            new Claim("Country",  user.Country),
        };

        var roles = await _userManager.GetRolesAsync(user);
        var userRoles = new List<string>();
        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
            userRoles.Add(role);
        }
        claims.Add(new Claim("RolesStr", string.Join(",", userRoles)));

        return claims;
    }
    private JwtSecurityToken GenerateTokenOptions(SigningCredentials signingCredentials, List<Claim> claims)
    {
        //var jwtSettings = _configuration.GetSection("JwtSettings");
        var tokenOptions = new JwtSecurityToken(
            issuer: _jwtConfiguration.ValidIssuer,
            audience: _jwtConfiguration.ValidAudience,
            claims: claims,
            expires: DateTime.Now.AddDays(Convert.ToDouble(_jwtConfiguration.ExpiresIn)),
            signingCredentials: signingCredentials);
        return tokenOptions;
    }
    private void SendResetPasswordEmail(User user, Token token)
    {
        string emailLink = $"https://{_configuration["CLIENT_URL"]}/reset-password?token={token.Value}";
        //var message = _emailManager.GetResetPasswordEmailTemplate(emailLink, user.Email);
        var message = emailLink;

        _emailManager.SendSingleEmail(user.Email, message, "Reset Password");
    }

    private void ValidateUserToken(Token token)
    {
        if (token == null)
            throw new RestException(HttpStatusCode.BadRequest, "Invalid token");

        var isTokenValid = CustomToken.IsTokenValid(token);
        if (!isTokenValid)
            throw new RestException(HttpStatusCode.BadRequest, "Invalid Token");
    }

    private void CheckUserStatus(User user)
    {
        if (user.Status.Equals(EUserStatus.Deactivated.ToString(), StringComparison.OrdinalIgnoreCase))
            throw new RestException(HttpStatusCode.Unauthorized,
                "You cannot login at this time. Please contact your admin.");
        if (!user.IsActive || !user.EmailConfirmed || user.Status != EUserStatus.Active.ToString())
            throw new RestException(HttpStatusCode.NotFound,
                "User in inactive");

    }
}