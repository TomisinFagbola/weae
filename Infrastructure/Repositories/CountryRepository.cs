using Domain.Entities;
using Infrastructure.Contracts;
using Infrastructure.Data.DbContext;


namespace Infrastructure.Repositories
{
    public class CountryRepository : RepositoryBase<Country>, ICountryRepository
    {
        public CountryRepository(AppDbContext appDbContext) : base(appDbContext)
        {

        }
    }
}