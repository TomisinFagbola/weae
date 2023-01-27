using Domain.Enums;
using Domain.Common;
using Domain.Entities.Identities;

namespace Domain.Entities
{
    public class Weather : AuditableEntity
    {
        public  Guid Id { get; set; }

        public string LowTemparature { get; set; }

        public string HighTemperature { get; set; }

        public string Humidity { get; set; }

        public string Pressure { get; set; }

        public string Type { get; set; } = EWeatherType.Sunny.ToString();

        public State State { get; set; }

        public Guid StateId { get; set; }

        

    }
}
