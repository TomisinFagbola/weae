using System.ComponentModel;

namespace Domain.Enums
{
    public enum EWeatherType
    {
        [Description("Sunny")]
        Sunny,
        [Description("Cloudy")]
        Cloudy,
        [Description("Rainy")]
        Rainy,
        [Description("Stormy")]
        Stormy,

    }
}
