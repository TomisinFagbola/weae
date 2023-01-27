using Infrastructure.Contracts;
using Infrastructure.Contracts.Identities;
using Infrastructure.Data.DbContext;
using Infrastructure.Repositories;
using Infrastructure.Repositories.Identities;

namespace Infrastructure;
public class RepositoryManager : IRepositoryManager
{
    private readonly AppDbContext _appDbContext;
    private readonly Lazy<IWeatherRepository> _weatherRepository;
    private readonly Lazy<IUserActivityRepository> _userActivityRepository;
    private readonly Lazy<ITokenRepository> _tokenRepository;
    private readonly Lazy<IUserRepository> _userRepository;
    private readonly Lazy<IStateRepository> _stateRepository;
    private readonly Lazy<ICountryRepository> _countryRepository;
    private readonly Lazy<IRoleRepository> _roleRepository;
    private readonly Lazy<IUserRoleRepository> _userRoleRepository;

    public RepositoryManager(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
        _weatherRepository = new Lazy<IWeatherRepository>(() => new WeatherRepository(appDbContext));
        _userActivityRepository = new Lazy<IUserActivityRepository>(() => new UserActivityRepository(appDbContext));
        _tokenRepository = new Lazy<ITokenRepository>(() => new TokenRepository(appDbContext));
        _userRepository = new Lazy<IUserRepository>(() => new UserRepository(appDbContext));
        _stateRepository = new Lazy<IStateRepository>(() => new StateRepository(appDbContext));
        _countryRepository = new Lazy<ICountryRepository>(() => new CountryRepository(appDbContext));
        _roleRepository =  new Lazy<IRoleRepository>(() => new RoleRepository(appDbContext));
        _userRoleRepository = new Lazy<IUserRoleRepository>(() => new UserRoleRepository(appDbContext));


    }

    public IWeatherRepository Weather => _weatherRepository.Value;
    public IUserActivityRepository UserActivity => _userActivityRepository.Value;

    public ICountryRepository Country => _countryRepository.Value;

    public IRoleRepository Role => _roleRepository.Value;

    public IUserRoleRepository UserRole => _userRoleRepository.Value;

    public ITokenRepository Token => _tokenRepository.Value;

    public IUserRepository User => _userRepository.Value;

    public IStateRepository State => _stateRepository.Value;
    public async Task SaveChangesAsync() => await _appDbContext.SaveChangesAsync();
}