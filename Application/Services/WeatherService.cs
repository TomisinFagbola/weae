using Application.Contracts;
using Application.DataTransferObjects;
using Application.Helpers;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain.Entities;
using Infrastructure;
using Infrastructure.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class WeatherService : IWeatherService
    {
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;

    

    public WeatherService(IRepositoryManager repository,
            IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

        public async Task<PagedResponse<IEnumerable<WeatherDto>>> GetStatesWeather(StateParameter parameters, string actionName, IUrlHelper urlHelper)
        {
            var weatherQuery = _repository.Weather.QueryAll().Include(x => x.State) as IQueryable<Weather>; 

            if (!string.IsNullOrEmpty(parameters.Search)) 
            {
                weatherQuery = (IOrderedQueryable<Weather>)weatherQuery.Where(x =>
                x.State.Name.Contains(parameters.Search.ToLower()));
            }

            var weatherDtos = weatherQuery.ProjectTo<WeatherDto>(_mapper.ConfigurationProvider);

            var pagedWeather = await PagedList<WeatherDto>.CreateAsync(weatherDtos, parameters.PageNumber, parameters.PageSize, parameters.Sort);

            var dynamicParameters = PageUtility<WeatherDto>.GenerateResourceParameters(parameters, pagedWeather);

            var page = PageUtility<WeatherDto>.CreateResourcePageUrl(dynamicParameters, actionName, pagedWeather, urlHelper);

            return new PagedResponse<IEnumerable<WeatherDto>>
            {
                Message = "Weather for each state retrieved successfully",
                Data = pagedWeather,
                Success = true,
                Meta = new Meta
                {
                    Pagination = page
                }
            };

        }

        public async Task<SuccessResponse<WeatherDto>> GetWeatherByState(Guid id)
        {
            var state = GetState(id);
            Guard.AgainstNull(state);

            var weather = await _repository.Weather.Get(x => x.StateId == id).FirstOrDefaultAsync();

            var response = _mapper.Map<WeatherDto>(weather);

            return new SuccessResponse<WeatherDto>
            {
                Data = response,
                Message = "Data successfully retrieved",
                Success = true,
            };
        }

    

        public async Task<SuccessResponse<WeatherDto>> RegisterWeatherForState(WeatherCreateDto weatherModel, Guid id)
        {
            var state = GetState(id);
            Guard.AgainstNull(state);

            var weatherToCreate = await ValidateWeather(weatherModel, id);

            var weather = _mapper.Map<Weather>(weatherToCreate);

            await _repository.Weather.AddAsync(weather);
      

            await _repository.SaveChangesAsync();

            var weatherResponse = _mapper.Map<WeatherDto>(weather);

            return new SuccessResponse<WeatherDto>
            {
                Data = weatherResponse,
                Message = "Weather for state successfully created",
                Success = true

            };

        }

        public async Task RemoveWeather(Guid id)
        {
                var state = await _repository.Weather.Get(x => x.Id == id).FirstOrDefaultAsync();
                Guard.AgainstNull(state);

                _repository.Weather.Remove(state);
                await _repository.SaveChangesAsync();
            
        }

        public async Task<SuccessResponse<WeatherDto>> UpdateWeather(WeatherUpdateDto model, Guid id)
        {

            var weather = await _repository.Weather.Get(x => x.Id == id).FirstOrDefaultAsync();
            Guard.AgainstNull(weather);


            var weatherToUpdate = _mapper.Map(model, weather);

            _repository.Weather.Update(weatherToUpdate);
            await _repository.SaveChangesAsync();

            var weatherUpdated = _mapper.Map<WeatherDto>(weatherToUpdate);
            return new SuccessResponse<WeatherDto>
            {
                Data = weatherUpdated,
                Message = "Weather successfully Updated",
                Success = true,
            };
        }




         #region Reusuables
        private async Task<WeatherCreateDto> ValidateWeather(WeatherCreateDto model, Guid id)
        {
            var weather = await _repository.Weather.Get(x => x.LowTemparature == model.LowTemparature && x.HighTemperature == model.HighTemperature && x.Pressure == model.Pressure && x.Humidity == model.Humidity && x.StateId == id && x.Type.ToLower() == model.Type.ToLower()).FirstOrDefaultAsync();

            Guard.AgainstDuplicate(weather);
            return model;

            
        }

        private async Task<State> GetState(Guid id)
        {
            var state = await _repository.State.Get(x => x.Id == id).FirstOrDefaultAsync();
            return state;
        }

        private async Task<Weather> GetWeather(Guid id)
        {
            var weather = await _repository.Weather.Get(x => x.Id == id).FirstOrDefaultAsync();
            return weather;
        }
        #endregion
    }
}

