using Application.Contracts;
using Application.DataTransferObjects;
using Application.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;


namespace Presentation.Controllers
{

    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/states")]
    public class WeatherController : ControllerBase
    {

        private readonly IServiceManager _service;

        public WeatherController(IServiceManager service)
        {
            _service = service;
        }


        /// <summary>
        /// Endpoint to register weather for a state
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        [HttpPost("{id}/weather")]
        [ProducesResponseType(typeof(SuccessResponse<WeatherDto>), 200)]
        public async Task<IActionResult> RegisterWeather([FromForm] WeatherCreateDto weatherModel, Guid id)
        {
            var response = await _service.WeatherService.RegisterWeatherForState(weatherModel, id);
            return Ok(response);
        }


        /// <summary>
        /// Endpoint to get weather by Stateid
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin, Regular")]
        [HttpGet("{id}/weather")]
        [ProducesResponseType(typeof(SuccessResponse<WeatherDto>), 200)]
        public async Task<IActionResult> GetWeather(Guid id)
        {
            var response = await _service.WeatherService.GetWeatherByState(id);
            return Ok(response);
        }

        /// <summary>
        /// Endpoint to get states weather
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "Admin, Regular")]
        [HttpGet(Name = nameof(GetWeather))]
        [ProducesResponseType(typeof(PagedResponse<ICollection<WeatherDto>>), 200)]
        public async Task<IActionResult> GetStatesWeather([FromQuery]StateParameter parameters)
        {
            var response = await _service.WeatherService.GetStatesWeather(parameters, nameof(GetWeather), Url);
            return Ok(response);
        }


     

        /// <summary>
        /// Endpoint to Update weather for state
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        [HttpPut("{id}/weather")]
        [ProducesResponseType(typeof(SuccessResponse<WeatherDto>), 200)]
        public async Task<IActionResult> UpdateWeather([FromForm] WeatherUpdateDto model, Guid id)
        {
            var response = await _service.WeatherService.UpdateWeather(model, id);
            return Ok(response);
        }


        // <summary>
        /// Endpoint to remove weather for state 
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}/weather")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        public async Task<IActionResult> RemoveWeather(Guid Id)
        {
            await _service.WeatherService.RemoveWeather(Id);
            return NoContent();
        }
    }
}


