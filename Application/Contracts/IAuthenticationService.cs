using Application.DataTransferObjects;
using Application.Helpers;

namespace Application.Contracts;

public interface IAuthenticationService
{
    Task<SuccessResponse<AuthDto>> Login(UserLoginDTO model);
    Task<SuccessResponse<AuthDto>> RefreshToken(RefreshTokenDTO model);
    Task<TokenResponse<object>> VerifyToken(string token);
    Task<SuccessResponse<object>> SetPassword(SetPasswordDTO model);
}