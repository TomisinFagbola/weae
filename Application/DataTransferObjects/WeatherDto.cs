using Domain.Entities.Identities;
using Domain.Entities;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DataTransferObjects
{
    public record WeatherDto
    {
        public Guid Id { get; set; }

        public string LowTemparature { get; set; }

        public string HighTemperature { get; set; }

        public string Humidity { get; set; }

        public string Pressure { get; set; }

        public string Type { get; set; }

        public State State { get; set; }




    }

    public record WeatherCreateDto : WeatherDto
    {
    }

    public record WeatherUpdateDto : WeatherCreateDto
    {
    }
}
