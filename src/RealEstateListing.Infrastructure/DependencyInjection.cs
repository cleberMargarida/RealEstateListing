using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RealEstateListing.Domain.Repositories;
using RealEstateListing.Infrastructure.Data;

namespace RealEstateListing.Infrastructure;

/// <summary>
/// Extension methods for configuring Infrastructure layer services.
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Adds Infrastructure layer services to the dependency injection container.
    /// Configures EF Core with SQL Server provider using the "SqlServer" connection string.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <returns>The service collection for chaining.</returns>
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddDbContext<ApplicationDbContext>((serviceProvider, options) =>
        {
            var connectionString = serviceProvider
                .GetRequiredService<IConfiguration>()
                .GetConnectionString("SqlServer");

            options.UseSqlServer(connectionString);
            
            if (serviceProvider.GetRequiredService<IWebHostEnvironment>().IsDevelopment())
            {
                options.EnableSensitiveDataLogging()
                       .EnableDetailedErrors();
            }
        });

        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }
}
