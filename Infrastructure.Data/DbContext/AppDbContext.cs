using Domain.Entities;
using Domain.Entities.Identities;
using Infrastructure.Data.DbContext.Configurations;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Diagnostics.Metrics;
using System.IO;

namespace Infrastructure.Data.DbContext;

public class AppDbContext : IdentityDbContext<User, Role, Guid,
        UserClaim, UserRole, UserLogin, RoleClaim, UserToken>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(WeatherConfiguration).Assembly);
    }

    public DbSet<State> States { get; set; }
    
    public DbSet<Token> Tokens { get; set; }

    public DbSet<Country> Countries { get; set; }

    public DbSet<Weather> Weather { get; set; }
}


