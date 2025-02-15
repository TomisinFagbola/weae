﻿using Application.Contracts;
using Application.DataTransferObjects;
using Application.Helpers;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace Presentation.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IServiceManager _service;

        public AuthController(IServiceManager service)
        {
            _service = service;
        }


        /// <summary>
        /// Logs in user with email and password
        /// </summary>
        /// <param name="model"></param>
        /// <returns>Jwt Token</returns>
        [HttpPost("login")]
        //[Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(SuccessResponse<AuthDto>), 200)]
        public async Task<IActionResult> Authenticate([FromBody] UserLoginDTO model)
        {
            var response = await _service.AuthenticationService.Login(model);
            return Ok(response);
        }
        /// <summary>
        /// Refresh access token
        /// </summary>
        /// <param name="tokenDto"></param>
        /// <returns></returns>
        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenDTO tokenDto)
        {
            var response = await _service.AuthenticationService.RefreshToken(tokenDto);
            return Ok(response);
        }


        /// <summary>
        /// Endpoint to verify token
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>

        [HttpGet]
        [Route("verify-token")]
        [ProducesResponseType(typeof(TokenResponse<object>), 200)]
        public async Task<IActionResult> VerifyToken([FromQuery] string token)
        {
            var response = await _service.AuthenticationService.VerifyToken(token);
            return Ok(response);
        }

        /// <summary>
        /// Endpoint to set the user password
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>

        [HttpPost]
        [Route("set-password")]
        [ProducesResponseType(typeof(SuccessResponse<object>), 200)]
        public async Task<IActionResult> SetPassword(SetPasswordDTO model)
        {
            var response = await _service.AuthenticationService.SetPassword(model);

            return Ok(response);
        }
    }
}