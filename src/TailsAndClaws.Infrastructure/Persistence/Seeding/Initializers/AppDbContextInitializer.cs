using Bogus;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TailsAndClaws.Domain.Entities;
using TailsAndClaws.Infrastructure.Persistence.Constants;

namespace TailsAndClaws.Infrastructure.Persistence.Seeding.Initializers;

public class AppDbContextInitializer(
    ILogger<AppDbContextInitializer> logger,
    AppDbContext context,
    Faker<Dog> dogFaker)
    : IAppDbContextInitializer
{
    private readonly ILogger<AppDbContextInitializer> _logger = logger;
    private readonly AppDbContext _context = context;
    private readonly Faker<Dog> _dogFaker = dogFaker;

    public List<Dog> Dogs { get; } = [];

    public async Task ApplyDatabaseStructure()
    {
        try
        {
            await _context.Database.MigrateAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ErrorMessagesConstants.MigrationError);
            throw;
        }
    }

    public async Task SeedAsync(int dogsAmount = 5)
    {
        if (!await IsEmpty())
        {
            return;
        }

        try
        {
            Dogs.Clear();

            await SeedDogs(dogsAmount);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ErrorMessagesConstants.SeedingError);
            throw;
        }
    }

    public async Task BulkClearStorageAsync()
    {
        await _context.Dogs
            .ExecuteDeleteAsync();
    }

    public Task ClearStorageAsync()
    {
        _context.Dogs.RemoveRange(_context.Dogs);
        return _context.SaveChangesAsync();
    }

    private async Task SeedDogs(int amount)
    {
        Dogs.AddRange(_dogFaker.Generate(amount));
        await _context.Dogs.AddRangeAsync(Dogs);

        await _context.SaveChangesAsync();
    }

    private async Task<bool> IsEmpty()
    {
        return !await _context.Dogs.AnyAsync();
    }

    public void Dispose()
    {
        _context.Dispose();
    }

    public async ValueTask DisposeAsync()
    {
        await _context.DisposeAsync();
    }
}
