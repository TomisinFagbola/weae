using Domain.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DataTransferObjects
{
    public record CountryDto
    {
        public string Name { get; set; }

        public string Continent { get; set; }

        public ICollection<State> States { get; set; }
    }

    public record CountryCreateDto : CountryDto
    {
    }

    public record CountryUpdateDto : CountryCreateDto
    {
    }
}
