using AMSaiian.Shared.Application;
using AMSaiian.Shared.Application.Behaviours;
using AMSaiian.Shared.Application.Factories;
using AMSaiian.Shared.Application.Interfaces;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace TailsAndClaws.Application;

public static class ConfigureServices
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services
            .AddHandlersAndBehaviour()
            .AddFluentValidators()
            .AddMapping()
            .AddCustomServices();

        return services;
    }

    private static IServiceCollection AddHandlersAndBehaviour(this IServiceCollection services)
    {
        services.AddMediatR(options =>
        {
            options.RegisterServicesFromAssembly(ApplicationAssemblyType.Reference);
            options.AddOpenBehavior(typeof(LoggingBehaviour<,>));
            options.AddOpenBehavior(typeof(ValidationBehaviour<,>));
        });

        return services;
    }

    private static IServiceCollection AddFluentValidators(this IServiceCollection services)
    {
        services
            .AddValidatorsFromAssembly(ApplicationAssemblyType.Reference)
            .AddValidatorsFromAssembly(SharedApplicationAssemblyType.Reference);

        return services;
    }

    private static IServiceCollection AddMapping(this IServiceCollection services)
    {
        services.AddAutoMapper(configuration =>
                                   configuration.AddMaps(ApplicationAssemblyType.Reference));

        return services;
    }

    private static IServiceCollection AddCustomServices(this IServiceCollection services)
    {
        services
            .AddSingleton<IOrderFactory, OrderFactory>();

        return services;
    }
}
