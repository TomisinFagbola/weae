using Application.DataTransferObjects;
using Application.Helpers;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts
{
    public interface ICountryService
    {
        Task<SuccessResponse<CountryDto>> RegisterCountry(CountryCreateDto model);

        Task<SuccessResponse<CountryDto>> GetCountryById(Guid id);

        Task<PagedResponse<IEnumerable<CountryDto>>> GetCountries(CountryParameters parameters, string actionName, IUrlHelper urlHelper);

        Task<SuccessResponse<CountryDto>> UpdateCountry(CountryUpdateDto model, Guid id);

        Task RemoveCountry(Guid id);


    }
}