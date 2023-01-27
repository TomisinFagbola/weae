using Application.DataTransferObjects;
using Application.Helpers;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Application.Contracts
{
    public interface IWeatherService
    {
        Task<PagedResponse<IEnumerable<WeatherDto>>> GetStatesWeather(StateParameter parameters, string actionName, IUrlHelper urlHelper);

        Task<SuccessResponse<WeatherDto>> GetWeatherByState(Guid id);

        Task<SuccessResponse<WeatherDto>> RegisterWeatherForState(WeatherCreateDto weatherModel, Guid id);

        Task RemoveWeather(Guid id);

        Task<SuccessResponse<WeatherDto>> UpdateWeather(WeatherUpdateDto model, Guid id);


    }
}
