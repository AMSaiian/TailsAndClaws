using TailsAndClaws.Domain.Entities;
using Task = System.Threading.Tasks.Task;

namespace TailsAndClaws.Infrastructure.Persistence.Seeding.Initializers;

public interface IAppDbContextInitializer
{
    public List<Dog> Dogs { get; }

    public Task ApplyDatabaseStructure();

    public Task SeedAsync(int dogsAmount = 5);

    public Task BulkClearStorageAsync();

    public Task ClearStorageAsync();
}
