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
    public class StateService : IStateService
    {
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;



        public StateService(IRepositoryManager repository,
                IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<PagedResponse<IEnumerable<StateDto>>> GetStates(Guid countryId, StateParameter parameters, string actionName, IUrlHelper urlHelper)
        {
            var country = await _repository.Country.Get(x => x.Id == countryId).FirstOrDefaultAsync();

            Guard.AgainstNull(country);

            var statesQuery = _repository.State.QueryAll(x => x.CountryId == country.Id) as IQueryable<State>;

            if (!string.IsNullOrEmpty(parameters.Search))
            {
                statesQuery = (IOrderedQueryable<State>)statesQuery.Where(x =>
                x.Name.Contains(parameters.Search.ToLower()));
            }

            var statesDtos = statesQuery.ProjectTo<StateDto>(_mapper.ConfigurationProvider);

            var pagedStates = await PagedList<StateDto>.CreateAsync(statesDtos, parameters.PageNumber, parameters.PageSize, parameters.Sort);

            var dynamicParameters = PageUtility<StateDto>.GenerateResourceParameters(parameters, pagedStates);

            var page = PageUtility<StateDto>.CreateResourcePageUrl(dynamicParameters, actionName, pagedStates, urlHelper);

            return new PagedResponse<IEnumerable<StateDto>>
            {
                Message = "States retrieved successfully",
                Data = pagedStates,
                Success = true,
                Meta = new Meta
                {
                    Pagination = page
                }
            };

        }

        public async Task<SuccessResponse<StateDto>> GetStateById(Guid id, Guid countryId)
        {
            var country = await _repository.Country.Get(x => x.Id == countryId).FirstOrDefaultAsync();
            Guard.AgainstNull(country);

            var state = await _repository.State.Get(x => x.CountryId == countryId && x.Id == id).FirstOrDefaultAsync();

            var response = _mapper.Map<StateDto>(state);

            return new SuccessResponse<StateDto>
            {
                Data = response,
                Message = "Data successfully retrieved",
                Success = true,
            };
        }



        public async Task<SuccessResponse<StateDto>> RegisterState(StateCreateDto model, Guid id)
        {
            var country = GetCountry(id);
            Guard.AgainstNull(country);

            var stateToCreate = await ValidateState(model, id);

            var state = _mapper.Map<State>(stateToCreate);

            await _repository.State.AddAsync(state);


            await _repository.SaveChangesAsync();

            var stateResponse = _mapper.Map<StateDto>(state);

            return new SuccessResponse<StateDto>
            {
                Data = stateResponse,
                Message = "Sate successfully created",
                Success = true

            };

        }

        public async Task RemoveState(Guid countryId, Guid id)
        {
            var country = await _repository.Country.Get(x => x.Id == countryId).FirstOrDefaultAsync();
            Guard.AgainstNull(country);

            var state = await _repository.State.Get(x => x.Id == id).FirstOrDefaultAsync();
            Guard.AgainstNull(state);

            _repository.State.Remove(state);
            await _repository.SaveChangesAsync();

        }

        public async Task<SuccessResponse<StateDto>> UpdateState(StateUpdateDto model, Guid countryId, Guid id)
        {

            var country = await _repository.Country.Get(x => x.Id == countryId).FirstOrDefaultAsync();
            Guard.AgainstNull(country);

            var state = await _repository.State.Get(x => x.Id == id && x.CountryId == country.Id).FirstOrDefaultAsync();
            Guard.AgainstNull(state);


            var stateToUpdate = _mapper.Map(model, state);

            _repository.State.Update(stateToUpdate);
            await _repository.SaveChangesAsync();

            var stateUpdated = _mapper.Map<StateDto>(stateToUpdate);
            return new SuccessResponse<StateDto>
            {
                Data = stateUpdated,
                Message = "State successfully Updated",
                Success = true,
            };
        }




        #region Reusuables
        private async Task<StateCreateDto> ValidateState(StateCreateDto model, Guid countryId)
        {
            var state = await _repository.State.Get(x => x.Name == model.Name && x.CountryId == countryId).FirstOrDefaultAsync();

            Guard.AgainstDuplicate(state);
            return model;


        }

        private async Task<Country> GetCountry(Guid id)
        {
            var country = await _repository.Country.Get(x => x.Id == id).FirstOrDefaultAsync();
            return country;
        }

        private async Task<Weather> GetWeather(Guid id)
        {
            var weather = await _repository.Weather.Get(x => x.Id == id).FirstOrDefaultAsync();
            return weather;
        }
        #endregion
    }
}
