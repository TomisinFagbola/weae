using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts
{
    public interface IServiceManager
    {
        IAuthenticationService AuthenticationService { get;}
        IWeatherService WeatherService { get;}
        IStateService StateService { get; }
        ICountryService CountryService { get; }
        IUserService UserService { get; }


    }
}
