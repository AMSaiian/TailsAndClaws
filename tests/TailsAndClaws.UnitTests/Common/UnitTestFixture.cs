using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using TailsAndClaws.Application.Common.Mapping;
using TailsAndClaws.Infrastructure.Persistence;
using TailsAndClaws.Infrastructure.Persistence.Seeding.Fakers;
using TailsAndClaws.Infrastructure.Persistence.Seeding.Initializers;

namespace TailsAndClaws.UnitTests.Common;

public class UnitTestFixture : IDisposable, IAsyncDisposable
{
    public AppDbContext DbContext { get; private set;  }

    public IAppDbContextInitializer AppDbContextInitializer { get; private set; }

    public IMapper Mapper { get; }

    public UnitTestFixture()
    {
        DbContext = BuildDbContext();

        AppDbContextInitializer = BuildAppDbContextInitializer();

        var config = new MapperConfiguration(
            cfg =>
                cfg.AddProfiles(
                [
                    new DogProfile(),
                ]));
        Mapper = config.CreateMapper();
    }

    private AppDbContext BuildDbContext()
    {
        return new AppDbContext(
            new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options);
    }

    private AppDbContextInitializer BuildAppDbContextInitializer()
    {
        return new AppDbContextInitializer(Mock.Of<ILogger<AppDbContextInitializer>>(),
                                           DbContext,
                                           new DogFaker());
    }

    public void Dispose()
    {
        AppDbContextInitializer.Dispose();
    }

    public async ValueTask DisposeAsync()
    {
        await AppDbContextInitializer.DisposeAsync();
    }
}

[CollectionDefinition(CollectionName)]
public class UnitTestsCollectionDefinition : ICollectionFixture<UnitTestFixture>
{
    public const string CollectionName = "UnitTestsWithObviousDependenciesCollection";
}
