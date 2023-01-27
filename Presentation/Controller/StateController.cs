using Application.Contracts;
using Application.DataTransferObjects;
using Application.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using System.Net;
using Domain.Entities;

namespace Presentation.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/countries")]
    public class StatesController : ControllerBase
    {
        private readonly IServiceManager _service;
        public StatesController(IServiceManager service)
        {
            _service = service;
        }

        /// <summary>
        /// Endpoint to get a state in a country
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin, Regular")]
        [HttpGet("{countryId}/states/{id}")]
        [ProducesResponseType(typeof(SuccessResponse<StateDto>), 200)]
        public async Task<IActionResult> State(Guid countryId, Guid id )
        {
            var response = await _service.StateService.GetStateById(countryId, id);
            return Ok(response);
        }

        /// <summary>
        /// Endpoint to get states
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin, Regular")]
        [HttpGet("{countryId}/states", Name = nameof(GetStates))]
        [ProducesResponseType(typeof(PagedResponse<IEnumerable<StateDto>>), 200)]
        public async Task<IActionResult> GetStates([FromQuery] StateParameter parameters, Guid countryId)
        {
            var response = await _service.StateService.GetStates(countryId, parameters, nameof(GetStates), Url);
            return Ok(response);
        }

        /// <summary>
        /// Endpoint to update state
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        [HttpPut("{countryId}/states/{id}")]
        [ProducesResponseType(typeof(SuccessResponse<StateDto>), 200)]
        public async Task<IActionResult> UpdateState([FromForm]StateUpdateDto model, Guid countryId, Guid id)
        {
            var response = await _service.StateService.UpdateState(model, countryId, id);
            return Ok(response);
        }

        

        // <summary>
        /// Endpoint to Remove state
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        [HttpDelete("{countryId}/states/{id}")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        public async Task<IActionResult> RemoveState(Guid countryId, Guid id)
         {
            await _service.StateService.RemoveState(countryId, id);
            return NoContent();
        }
    } 
}
