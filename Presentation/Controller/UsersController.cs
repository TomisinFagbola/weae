using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Application.Helpers;
using Application.Contracts;
using Application.DataTransferObjects;
using System;

namespace API.Controllers
{
    [Authorize]
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/users")]
    public class UserController : ControllerBase
    {
        private readonly IServiceManager _service;

        public UserController(IServiceManager service)
        {
            _service = service;
        }

        /// <summary>
        /// Endpoint to get a user
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(SuccessResponse<UserDTO>), 200)]
        public async Task<IActionResult> GetUserById(Guid id)
        {
            var response = await _service.UserService.GetUserById(id);

            return Ok(response);
        }

        /// <summary>
        /// Endpoint to invite a new user
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [Route("invite")]
        [ProducesResponseType(typeof(SuccessResponse<UserDTO>), 200)]
        public async Task<IActionResult> RegisterUser([FromBody] UserCreateDTO model)
        {
            var response = await _service.UserService.RegisterUser(model);
            return Ok(response);
        }



        /// <summary>
        /// Endpoint to get the list of users
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        [HttpGet(Name = nameof(GetAllUsers))]
        public async Task<IActionResult> GetAllUsers([FromQuery] UserParameters parameters)
        {
            var result = await _service.UserService.GetUsers(parameters, nameof(GetAllUsers), Url);
            return Ok(result);
        }

    }
}