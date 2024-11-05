using AMSaiian.Shared.Web.Middlewares;
using Serilog;
using TailsAndClaws;
using TailsAndClaws.Application;
using TailsAndClaws.Common.Constants;
using TailsAndClaws.Infrastructure;
using TailsAndClaws.Infrastructure.Persistence.Seeding.Initializers;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddEnvironmentVariables();

// Add services to the container.
builder.Host.UseLogging(builder.Services, builder.Configuration);

builder.Services.AddApplicationServices();
builder.Services.AddApplicationInfrastructure(builder.Configuration, "Application");
builder.Services.AddApiServices(builder.Configuration);
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

var app = builder.Build();

// Initialise and seed database
using (var scope = app.Services.CreateScope())
{
    var appDbInitializer = scope.ServiceProvider
        .GetRequiredService<IAppDbContextInitializer>();

    await appDbInitializer.ApplyDatabaseStructure();

    if (bool.TryParse(builder.Configuration.GetSection(SeedingEnvConstants.SeedingEnabled).Value, out bool value) && value)
    {
        const int defaultDogsAmount = 5;
        int.TryParse(builder.Configuration.GetSection(SeedingEnvConstants.DogsSeedingAmount).Value, out int amount);

        amount = amount > 0
            ? amount
            : defaultDogsAmount;

        await appDbInitializer.SeedAsync(amount);
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.UseSerilogRequestLogging();
app.UseExceptionHandler();
app.UseRateLimiter();
app.MapControllers();

await app.RunAsync();

namespace TailsAndClaws
{
    public partial class Program;
}
