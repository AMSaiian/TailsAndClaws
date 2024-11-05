using AMSaiian.Shared.Application.Interfaces;
using AMSaiian.Shared.Infrastructure.Services;
using Bogus;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TailsAndClaws.Application.Common.Interfaces;
using TailsAndClaws.Domain.Entities;
using TailsAndClaws.Infrastructure.Persistence;
using TailsAndClaws.Infrastructure.Persistence.Seeding.Fakers;
using TailsAndClaws.Infrastructure.Persistence.Seeding.Initializers;

namespace TailsAndClaws.Infrastructure;

public static class ConfigureServices
{
    public static IServiceCollection AddApplicationInfrastructure(
        this IServiceCollection services,
        IConfigurationManager configuration,
        string appConnectionStringName,
        int seedingValue = 42)
    {
        string connectionString = configuration.GetConnectionString(appConnectionStringName)
                               ?? throw new ArgumentNullException(nameof(appConnectionStringName));

        services
            .AddAppDbContext(connectionString)
            .AddAppDbContextInitializer(seedingValue);

        return services;
    }

    public static IServiceCollection AddAppDbContext(
        this IServiceCollection services,
        string connectionString)
    {
        services
            .AddDbContext<AppDbContext>(options =>
            {
                options
                    .UseNpgsql(connectionString)
                    .EnableSensitiveDataLogging(false);
            })
            .AddSingleton<IPaginationService, PaginationService>()
            .AddScoped<IAppDbContext, AppDbContext>();

        return services;
    }

    private static IServiceCollection AddAppDbContextInitializer(
        this IServiceCollection services,
        int seedingValue)
    {
        Randomizer.Seed = new Random(seedingValue);

        services
            .AddScoped<IAppDbContextInitializer, AppDbContextInitializer>()
            .AddScoped<Faker<Dog>, DogFaker>();

        return services;
    }
}
