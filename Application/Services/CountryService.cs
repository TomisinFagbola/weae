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
    public class CountryService : ICountryService
    {
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;



        public CountryService(IRepositoryManager repository,
                IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<PagedResponse<IEnumerable<CountryDto>>> GetCountries(CountryParameters parameters, string actionName, IUrlHelper urlHelper)
        {
            

            var countriesQuery = _repository.Country.QueryAll().Include(x => x.States) as IQueryable<Country>;

            if (!string.IsNullOrEmpty(parameters.Search))
            {
                countriesQuery = (IOrderedQueryable<Country>)countriesQuery.Where(x =>
                x.Name.Contains(parameters.Search.ToLower()));
            }

            var countriesDtos = countriesQuery.ProjectTo<CountryDto>(_mapper.ConfigurationProvider);

            var pagedCountries = await PagedList<CountryDto>.CreateAsync(countriesDtos, parameters.PageNumber, parameters.PageSize, parameters.Sort);

            var dynamicParameters = PageUtility<CountryDto>.GenerateResourceParameters(parameters, pagedCountries);

            var page = PageUtility<CountryDto>.CreateResourcePageUrl(dynamicParameters, actionName, pagedCountries, urlHelper);

            return new PagedResponse<IEnumerable<CountryDto>>
            {
                Message = "States retrieved successfully",
                Data = pagedCountries,
                Success = true,
                Meta = new Meta
                {
                    Pagination = page
                }
            };

        }

        public async Task<SuccessResponse<CountryDto>> GetCountryById(Guid countryId)
        {
            var country = await _repository.Country.Get(x => x.Id == countryId).FirstOrDefaultAsync();
            Guard.AgainstNull(country);
            
            var response = _mapper.Map<CountryDto>(country);

            return new SuccessResponse<CountryDto>
            {
                Data = response,
                Message = "Data successfully retrieved",
                Success = true,
            };
        }



        public async Task<SuccessResponse<CountryDto>> RegisterCountry(CountryCreateDto model)
        {
           

            var countryToCreate = await ValidateCountry(model);

            var country = _mapper.Map<Country>(countryToCreate);

            await _repository.Country.AddAsync(country);


            await _repository.SaveChangesAsync();

            var countryResponse = _mapper.Map<CountryDto>(country);

            return new SuccessResponse<CountryDto>
            {
                Data = countryResponse,
                Message = "Country successfully created",
                Success = true

            };

        }

        public async Task RemoveCountry(Guid id)
        {
            var country = await _repository.Country.Get(x => x.Id == id).FirstOrDefaultAsync();
            Guard.AgainstNull(country);

            _repository.Country.Remove(country);
            await _repository.SaveChangesAsync();

        }

        public async Task<SuccessResponse<CountryDto>> UpdateCountry(CountryUpdateDto model, Guid id)
        {

            var country = await _repository.Country.Get(x => x.Id == id).FirstOrDefaultAsync();
            Guard.AgainstNull(country);


            var countryToUpdate = _mapper.Map(model, country);

            _repository.Country.Update(countryToUpdate);
            await _repository.SaveChangesAsync();

            var countryUpdated = _mapper.Map<CountryDto>(countryToUpdate);
            return new SuccessResponse<CountryDto>
            {
                Data = countryUpdated,
                Message = "Country successfully Updated",
                Success = true,
            };
        }




        #region Reusuables
        private async Task<CountryCreateDto> ValidateCountry(CountryCreateDto model)
        {
            var state = await _repository.Country.Get(x => x.Name == model.Name && x.Continent == model.Continent).FirstOrDefaultAsync();

            Guard.AgainstDuplicate(state);
            return model;


        }

     

        #endregion
    }
}