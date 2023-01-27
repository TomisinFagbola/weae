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
    public class CountriesController : ControllerBase
    {
        private readonly IServiceManager _service;
        public CountriesController(IServiceManager service)
        {
            _service = service;
        }

        /// <summary>
        /// Endpoint to get a country
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin, Regular")]
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(SuccessResponse<StateDto>), 200)]
        public async Task<IActionResult> Country(Guid id)
        {
            var response = await _service.CountryService.GetCountryById(id);
            return Ok(response);
        }

        /// <summary>
        /// Endpoint to get countries
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin, Regular")]
        [HttpGet(Name = nameof(GetCountries))]
        [ProducesResponseType(typeof(PagedResponse<IEnumerable<StateDto>>), 200)]
        public async Task<IActionResult> GetCountries([FromQuery] CountryParameters parameters)
        {
            var response = await _service.CountryService.GetCountries(parameters, nameof(GetCountries), Url);
            return Ok(response);
        }

        /// <summary>
        /// Endpoint to update country
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(SuccessResponse<StateDto>), 200)]
        public async Task<IActionResult> UpdateCountry([FromForm] CountryUpdateDto model, Guid id)
        {
            var response = await _service.CountryService.UpdateCountry(model, id);
            return Ok(response);
        }



        // <summary>
        /// Endpoint to Remove country
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        public async Task<IActionResult> RemoveCountry(Guid id)
        {
            await _service.CountryService.RemoveCountry(id);
            return NoContent();
        }
    }
}
