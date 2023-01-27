using Domain.Entities;
using System.IO;

namespace Infrastructure.Contracts
{
    public interface IWeatherRepository : IRepositoryBase<Weather>
    {
    }
}
