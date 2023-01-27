using API.Helpers;
using Infrastructure.Data;
using Infrastructure.Data.DbContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using Application.DataTransferObjects;

namespace API.ContextFactory
{

    public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            var config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .AddJsonFile($"appsettings.Development.json", reloadOnChange: true, optional: true)
            .AddEnvironmentVariables()
            .Build();

            DbContextOptionsBuilder<AppDbContext> builder;

           
            builder = new DbContextOptionsBuilder<AppDbContext>()
                    .UseSqlServer(config.GetConnectionString("DefaultConnection1"), b => b.MigrationsAssembly("API"));
            return new AppDbContext(builder.Options);
        }
    }
}