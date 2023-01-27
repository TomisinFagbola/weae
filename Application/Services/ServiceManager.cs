using Application.Contracts;
using AutoMapper;
using Domain.Entities.Identities;
using Infrastructure.Contracts;
using Infrastructure.Utils.Email;
using Infrastructure.Utils.Logger;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;

namespace Application.Services;

public class ServiceManager : IServiceManager
{
    private readonly Lazy<IWeatherService> _weatherService;
    private readonly Lazy<IAuthenticationService> _authenticationService;
    private readonly Lazy<IStateService> _stateService;
    private readonly Lazy<ICountryService> _countryService;
    private readonly Lazy<IUserService> _userService;

    public ServiceManager(IRepositoryManager repositoryManager, ILoggerManager logger,
        IMapper mapper,
        UserManager<User> userManager,
        RoleManager<Role> roleManager,
        IEmailManager emailManager,
        IConfiguration configuration)
    {
        _weatherService = new Lazy<IWeatherService>(() => new WeatherService(repositoryManager, mapper));
        _authenticationService = new Lazy<IAuthenticationService>(() => new AuthenticationService(repositoryManager, userManager, emailManager, mapper, logger, configuration));

        _userService = new Lazy<IUserService>(() => new UserService(repositoryManager, userManager, roleManager, emailManager, mapper, logger, configuration));
        _stateService = new Lazy<IStateService>(() => new StateService(repositoryManager, mapper));
        _countryService = new Lazy<ICountryService>(() => new CountryService(repositoryManager, mapper));
    }



    public IWeatherService WeatherService => _weatherService.Value;
    public IAuthenticationService AuthenticationService => _authenticationService.Value;

    public IUserService UserService => _userService.Value;

    public IStateService StateService => _stateService.Value;

    public ICountryService CountryService => _countryService.Value;
    
}
