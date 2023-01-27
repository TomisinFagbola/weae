
using Infrastructure.Contracts.Identities;

namespace Infrastructure.Contracts;

public interface IRepositoryManager
{
    IWeatherRepository Weather { get; }

    IUserActivityRepository UserActivity { get; }

    ITokenRepository Token { get; }

    IUserRepository User { get; }

    IStateRepository State { get; }

    ICountryRepository Country { get;  }

    IRoleRepository Role { get; }

    IUserRoleRepository UserRole { get; }

    Task SaveChangesAsync();
}