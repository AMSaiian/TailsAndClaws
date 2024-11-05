using AMSaiian.Shared.Application.Exceptions;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using TailsAndClaws.Application.Dogs.Commands.CreateDog;
using TailsAndClaws.Domain.Entities;
using TailsAndClaws.UnitTests.Common;

namespace TailsAndClaws.UnitTests.Dogs;

[Collection(UnitTestsCollectionDefinition.CollectionName)]
public class CreateDogHandlerTests : IAsyncDisposable
{
    private readonly UnitTestFixture _fixture;
    private readonly CreateDogHandler _handler;

    public CreateDogHandlerTests(UnitTestFixture fixture)
    {
        var loggerMock = new Mock<ILogger<CreateDogHandler>>();
        _fixture = fixture;
        _handler = new CreateDogHandler(
            _fixture.DbContext,
            _fixture.Mapper,
            loggerMock.Object);

        _fixture.AppDbContextInitializer.SeedAsync();
    }

    [Fact]
    public async Task CreateDogHandlerCreatesNewDog()
    {
        // Arrange
        var command = new CreateDogCommand
        {
            Name = "Test Dog",
            Color = "Blue",
            WeightInKg = 1.145M,
            TailLengthInMeters = 0.554M,
        };

        // Act & Assert
        Func<Task<Guid>> action = async () => await _handler.Handle(command, CancellationToken.None);
        (await action
                .Should()
                .NotThrowAsync())
            .Subject
            .Should()
            .NotBeEmpty();
    }

    [Fact]
    public async Task CreateDogHandlerThrowsConflictIfDogWithSameNameExists()
    {
        // Arrange
        int initialDogAmount = await _fixture.DbContext.Dogs.CountAsync();
        Dog existingDog = _fixture.AppDbContextInitializer.Dogs[0];

        List<CreateDogCommand> commandsWithDifferentNameCases =
        [
            new()
            {
                Name = existingDog.Name.ToLowerInvariant(),
                Color = "Red",
                WeightInKg = 0.954M,
                TailLengthInMeters = 0.123M,
            },
            new()
            {
                Name = existingDog.Name.ToUpperInvariant(),
                Color = "Red",
                WeightInKg = 0.954M,
                TailLengthInMeters = 0.123M,
            },
            new()
            {
                Name = existingDog.Name,
                Color = "Red",
                WeightInKg = 0.954M,
                TailLengthInMeters = 0.123M,
            }
        ];

        // Act & Assert
        foreach (var command in commandsWithDifferentNameCases)
        {
            Func<Task<Guid>> action = async () => await _handler.Handle(command, CancellationToken.None);

            await action
                .Should()
                .ThrowExactlyAsync<ConflictException>();

            int currentDogsAmount = await _fixture.DbContext.Dogs
                .CountAsync();

            int dogWithExpectedName = await _fixture.DbContext.Dogs
                .CountAsync(dog => dog.NormalizedName == existingDog.NormalizedName);

            currentDogsAmount
                .Should()
                .Be(initialDogAmount);

            dogWithExpectedName
                .Should()
                .Be(1);
        }
    }

    public async ValueTask DisposeAsync()
    {
        await _fixture.AppDbContextInitializer.ClearStorageAsync();
    }
}
