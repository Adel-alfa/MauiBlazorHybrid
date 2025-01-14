using MauiBlazorHybrid.Api.Data.Entitties;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace MauiBlazorHybrid.Api.Data
{
    public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
            optionsBuilder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));

           
            var serviceProvider = new ServiceCollection()
                .AddLogging()
                .AddSingleton<IPasswordHasher<User>, PasswordHasher<User>>() 
                .BuildServiceProvider();

            return new AppDbContext(optionsBuilder.Options, serviceProvider.GetService<IPasswordHasher<User>>()!);
        }
    }
}
