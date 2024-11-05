using AMSaiian.Shared.Application.Interfaces;
using AMSaiian.Shared.Application.Models.Pagination;
using AMSaiian.Shared.Infrastructure.Services;
using AMSaiian.Shared.UnitTests.Common.SampleClasses;
using AutoBogus;
using FluentAssertions;
using MockQueryable;
using Moq;

namespace AMSaiian.Shared.UnitTests.Tests;

public class PaginationServiceTests
{
    private readonly Mock<IOrderFactory> _factoryMock;
    private readonly IPaginationService _paginationService;

    public PaginationServiceTests()
    {
        _factoryMock = new Mock<IOrderFactory>();
        _paginationService = new PaginationService(_factoryMock.Object);
    }

    [Theory]
    [InlineData(1, 5, 20, 100)]
    [InlineData(2, 10, 10, 100)]
    [InlineData(2, 50, 2, 100)]
    [InlineData(1, 100, 1, 100)]
    public async Task GetPaginationInfoAsyncReturnCorrectData(
        int page,
        int size,
        int expectedTotalPages,
        int samplesAmount)
    {
        // Arrange
        IQueryable<SampleOrderingClass> queryMock = AutoFaker
            .Generate<SampleOrderingClass>(samplesAmount)
            .BuildMock();

        PaginationInfo expectedResult = new PaginationInfo()
        {
            PageNumber = page,
            PageSize = size,
            TotalPages = expectedTotalPages,
            TotalRecords = samplesAmount
        };

        var context = new PageContext
        {
            PageNumber = page,
            PageSize = size,
        };

        // Act
        PaginationInfo result = await _paginationService.GetPaginationInfoAsync(
            queryMock,
            context,
            CancellationToken.None);

        // Assert
        result
            .Should()
            .BeEquivalentTo(expectedResult);
    }

    [Theory]
    [InlineData(1, 5, 100)]
    [InlineData(2, 10, 100)]
    [InlineData(2, 50, 100)]
    [InlineData(1, 100, 100)]
    public Task GetChunkQueryMustReturnCorrectEntries(
        int page,
        int size,
        int samplesAmount)
    {
        // Arrange
        List<SampleOrderingClass> samples = AutoFaker
            .Generate<SampleOrderingClass>(samplesAmount);

        IQueryable<SampleOrderingClass> queryMock = samples
            .BuildMock();

        IQueryable<SampleOrderingClass> expectedResult = MapExpectedChunk(samples, page, size);

        var context = new PageContext
        {
            PageNumber = page,
            PageSize = size,
        };

        // Act
        IQueryable<SampleOrderingClass> result = _paginationService.GetChunkQuery(
            queryMock,
            context);

        // Assert
        result
            .Should()
            .BeEquivalentTo(expectedResult);

        return Task.CompletedTask;
    }

    [Theory]
    [InlineData(1, 5, 20, 100)]
    [InlineData(2, 10, 10, 100)]
    [InlineData(2, 50, 2, 100)]
    [InlineData(1, 100, 1, 100)]
    public async Task PaginateAsyncMustReturnCorrectChunkAndInfo(
        int page,
        int size,
        int expectedTotalPages,
        int samplesAmount)
    {
        // Arrange
        List<SampleOrderingClass> samples = AutoFaker
            .Generate<SampleOrderingClass>(samplesAmount);

        List<SampleOrderingClass> expectedSamples = MapExpectedChunk(
                samples.OrderBy(x => x.Textual),
                page,
                size)
            .ToList();

        IQueryable<SampleOrderingClass> queryMock = samples
            .BuildMock();

        _factoryMock
            .Setup(
                mock => mock.OrderDynamically(
                    It.IsAny<IQueryable<SampleOrderingClass>>(),
                    It.IsAny<OrderContext>()))
            .Returns(queryMock.OrderBy(x => x.Textual));

        Paginated<SampleOrderingClass> expectedResult = new()
        {
            PaginationInfo = new()
            {
                PageNumber = page,
                PageSize = size,
                TotalPages = expectedTotalPages,
                TotalRecords = samplesAmount
            },
            Records = expectedSamples
        };

        var context = new PaginationContext
        {
            OrderContext = new()
            {
                IsDescending = false,
                PropertyName = SampleOrderingClassConstants.OrderingBy.Textual
            },
            PageContext = new()
            {
                PageNumber = page,
                PageSize = size
            }
        };

        // Act
        Paginated<SampleOrderingClass> result = await _paginationService.PaginateAsync(
            queryMock,
            context,
            CancellationToken.None);

        // Assert
        result
            .Should()
            .BeEquivalentTo(expectedResult);
    }

    private IQueryable<SampleOrderingClass> MapExpectedChunk(
        IEnumerable<SampleOrderingClass> samples,
        int page,
        int size) => samples
            .Skip((page - 1) * size)
            .Take(size)
            .AsQueryable();
}
