using Application.DataTransferObjects;
using Application.Helpers;
using AutoMapper;
using Domain.Entities.Identities;
using Domain.Entities;
using Domain.Enums;
using Infrastructure.Contracts;
using Infrastructure.Utils.Email;
using Infrastructure.Utils.Logger;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Application.Contracts;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using Microsoft.AspNetCore.Rewrite;

namespace Application.Services
{
    public class UserService : IUserService
    {
        private readonly IRepositoryManager _repository;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly IEmailManager _emailManager;
        private readonly IMapper _mapper;
        private readonly ILoggerManager _logger;
        private readonly IConfiguration _configuration;



        public UserService(IRepositoryManager repository, UserManager<User> userManager, RoleManager<Role> roleManager, IEmailManager emailManager,
            IMapper mapper, ILoggerManager logger, IConfiguration configuration)
        {
            _repository = repository;
            _userManager = userManager;
            _roleManager = roleManager;
            _emailManager = emailManager;
            _mapper = mapper;
            _logger = logger;

        }
        public async Task<SuccessResponse<UserDTO>> RegisterUser(UserCreateDTO model)
        {

            await IsEmailExist(model);

            var user = _mapper.Map<User>(model);
            user.UserName = user.Email;
            user.Password = _userManager.PasswordHasher.HashPassword(user, "Password@@1");

            var result = await _userManager.CreateAsync(user, "Password@@1");
            Guard.AgainstFailedTransaction(result.Succeeded);

            // add user to role
            if (!await _userManager.IsInRoleAsync(user, ERole.Regular.ToString()))
                await _userManager.AddToRoleAsync(user, ERole.Regular.ToString());

            var token = CustomToken.GenerateRandomString(128);
            var tokenEntity = new Token
            {
                UserId = user.Id,
                Value = token,
                TokenType = ETokenType.InviteUser.ToString()
            };

            await _repository.Token.AddAsync(tokenEntity);

            await _repository.SaveChangesAsync();

            var userResponse = _mapper.Map<UserDTO>(user);

            SendUserSignUpEmail(user, token);

            return new SuccessResponse<UserDTO>
            {
                Data = userResponse,
                Message = "Invitation sent"
            };
        }

        public async Task<SuccessResponse<UserDTO>> GetUserById(Guid userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());

            if (user == null)
                throw new RestException(HttpStatusCode.NotFound, "User record not found");

            var userResponse = _mapper.Map<UserDTO>(user);

            return new SuccessResponse<UserDTO>
            {
                Message = "Data retrieved successfully",
                Data = userResponse
            };
        }

      

        public async Task<PagedResponse<IEnumerable<UserDTO>>> GetUsers(UserParameters parameters, string actionName, IUrlHelper urlHelper)
        {

            var role =  await _repository.Role.Get(x => x.Name == ERole.Regular.ToString()).FirstOrDefaultAsync();

            Guard.AgainstNull(role);
            var userRoles = _repository.UserRole.QueryAll(x => x.RoleId == role.Id);
            var users = _repository.User.QueryAll();

            //var usersQuery = _repository.User.QueryAll();
            var usersList = (from user in users
                             join userRole in userRoles on user.Id equals userRole.UserId
                             select new UserDTO
                             {
                                 Id = user.Id,
                                 FirstName = user.FirstName,
                                 MiddleName = user.MiddleName,
                                 LastName = user.LastName,
                                 PhoneNumber = user.PhoneNumber,
                                 Email = user.Email,

                             } );
            
            if (!string.IsNullOrWhiteSpace(parameters.Search))
            {
                var search = parameters.Search.Trim().ToLower();
                usersList.Where(
                a => a.FirstName.ToLower().Contains(search) || a.LastName.ToLower().Contains(search)
                         || a.MiddleName.ToLower().Contains(search)).OrderBy(x => x.CreatedAt);
            }


            //var usersDtos = userList.ProjectTo<UserDTO>(_mapper.ConfigurationProvider);
            var pagedUsers = await PagedList<UserDTO>.CreateAsync(usersList, parameters.PageNumber, parameters.PageSize, parameters.Sort);
            var dynamicParameters = PageUtility<UserDTO>.GenerateResourceParameters(parameters, pagedUsers);
            var page = PageUtility<UserDTO>.CreateResourcePageUrl(dynamicParameters, actionName, pagedUsers, urlHelper);

            return new PagedResponse<IEnumerable<UserDTO>>
            {
                Message = "Users data retrieved successfully",
                Data = pagedUsers,
                Meta = new Meta
                {
                    Pagination = page
                }
            };
        }

        #region
        private async Task IsEmailExist(UserCreateDTO model)
        {
            var email = model.Email.Trim().ToLower();

            var user = await _repository.User.Get(x => x.Email == email).FirstOrDefaultAsync();
            Guard.AgainstDuplicate(user, "Email address already exists");
        }

        private void SendUserSignUpEmail(User user, string token)
        {
            string emailLink =
                $"https://{_configuration["CLIENT_URL"]}/user-signup?token={token}";
            string adminName = $"{user.FirstName} {user.LastName}";
            //var message = _emailManager.GetConfirmEmailTemplate(emailLink, user.Email);
            var message = emailLink;

            string subject = "Confirm Email";

            _emailManager.SendSingleEmail(user.Email, message, subject);
        }


        #endregion
    }
}