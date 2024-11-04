using System.Text.Json;
using System.Threading.RateLimiting;
using AMSaiian.Shared.Web;
using AMSaiian.Shared.Web.Filters;
using AMSaiian.Shared.Web.Middlewares;
using AMSaiian.Shared.Web.Options;
using Microsoft.AspNetCore.RateLimiting;
using Serilog;
using TailsAndClaws.Common.Constants;
using TailsAndClaws.Common.Options;

namespace TailsAndClaws;

public static class ConfigureServices
{
    public static IHostBuilder UseLogging(
        this IHostBuilder hostBuilder,
        IServiceCollection services,
        IConfigurationManager configuration)
    {
        hostBuilder.UseSerilog((context, loggerConfiguration) =>
        {
            loggerConfiguration.ReadFrom.Configuration(context.Configuration);
        });

        return hostBuilder;
    }

    public static IServiceCollection AddApiServices(
        this IServiceCollection services,
        IConfigurationManager configuration)
    {
        services.AddHttpContextAccessor();

        services.AddProblemDetails();

        RateLimitOptions policyOptions = configuration
            .GetSection(RateLimitOptions.SectionName)
            .Get<RateLimitOptions>() ?? new RateLimitOptions();

        ArgumentNullException.ThrowIfNull(policyOptions);

        services.AddRateLimiter(rateLimiterOptions =>
        {
            rateLimiterOptions
                .AddFixedWindowLimiter(
                    ApiConstants.DefaultRateLimitingPolicyName,
                    options =>
                    {
                        options.PermitLimit = policyOptions.RequestsPerWindowAmount;
                        options.Window = TimeSpan.FromSeconds(policyOptions.WindowLengthInSeconds);
                        options.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
                    })
                .RejectionStatusCode = StatusCodes.Status429TooManyRequests;
        });

        services
            .Configure<RouteOptions>(options =>
            {
                options.LowercaseUrls = true;
                options.LowercaseQueryStrings = true;
            });
        services
            .Configure<RequestQueryOptions>(configuration
                                                .GetRequiredSection(RequestQueryOptions.SectionName));

        services.AddControllers(opts => opts.Filters.Add<ApiExceptionFilterAttribute>())
            .AddJsonOptions(opts =>
            {
                opts.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower;
            });

        services
            .AddExceptionHandler<GlobalExceptionHandler>()
            .AddEndpointsApiExplorer()
            .AddAutoMapper(mapperConfiguration =>
                               mapperConfiguration.AddMaps(ApiAssemblyType.Reference,
                                                           SharedWebAssemblyType.Reference));

        return services;
    }
}
