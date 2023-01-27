using Application.DataTransferObjects;
using Application.Helpers;
using Domain.Entities.Identities;
using Microsoft.AspNetCore.Mvc;


namespace Application.Contracts
{
    public interface IUserService
    {
        public Task<SuccessResponse<UserDTO>> RegisterUser(UserCreateDTO model);
        Task<SuccessResponse<UserDTO>> GetUserById(Guid userId);

        Task<PagedResponse<IEnumerable<UserDTO>>> GetUsers(UserParameters parameters, string actionName, IUrlHelper urlHelper);

    }
}