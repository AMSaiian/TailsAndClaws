using AMSaiian.Shared.Application.Exceptions;
using AMSaiian.Shared.Application.Factories;
using AMSaiian.Shared.Application.Interfaces;
using AMSaiian.Shared.Application.Models.Pagination;
using AMSaiian.Shared.UnitTests.Common.SampleClasses;
using AutoBogus;
using FluentAssertions;

namespace AMSaiian.Shared.UnitTests.Tests;

public class OrderFactoryTests(OrderFactory factory) : IClassFixture<OrderFactory>
{
    private readonly IOrderFactory _factory = factory;

    [Fact]
    public void OrderFactoryShouldThrowsIfTryOrderNotDefined()
    {
        // Arrange
        const int sampleAmount = 10;
        var context = new OrderContext
        {
            IsDescending = true,
            PropertyName = Guid.NewGuid().ToString()
        };

        List<SampleOrderingClass> samples = AutoFaker
            .Generate<SampleOrderingClass>(sampleAmount);

        // Act
        Func<IOrderedQueryable<SampleOrderingClass>> function =
            () => _factory.OrderDynamically(samples.AsQueryable(), context);

        // Assert
        function
            .Should()
            .ThrowExactly<ValidationException>();
    }

    [Theory]
    [InlineData(SampleOrderingClassConstants.OrderingBy.Number, true)]
    [InlineData(SampleOrderingClassConstants.OrderingBy.NumberNested, true)]
    [InlineData(SampleOrderingClassConstants.OrderingBy.Pointed, true)]
    [InlineData(SampleOrderingClassConstants.OrderingBy.PointedNested, true)]
    [InlineData(SampleOrderingClassConstants.OrderingBy.Precised, true)]
    [InlineData(SampleOrderingClassConstants.OrderingBy.PrecisedNested, true)]
    [InlineData(SampleOrderingClassConstants.OrderingBy.Textual, true)]
    [InlineData(SampleOrderingClassConstants.OrderingBy.TextualNested, true)]
    [InlineData(SampleOrderingClassConstants.OrderingBy.Number, false)]
    [InlineData(SampleOrderingClassConstants.OrderingBy.NumberNested, false)]
    [InlineData(SampleOrderingClassConstants.OrderingBy.Pointed, false)]
    [InlineData(SampleOrderingClassConstants.OrderingBy.PointedNested, false)]
    [InlineData(SampleOrderingClassConstants.OrderingBy.Precised, false)]
    [InlineData(SampleOrderingClassConstants.OrderingBy.PrecisedNested, false)]
    [InlineData(SampleOrderingClassConstants.OrderingBy.Textual, false)]
    [InlineData(SampleOrderingClassConstants.OrderingBy.TextualNested, false)]
    public void OrderFactoryShouldReturnCorrectlyOrderedQuery(
        string orderingBy,
        bool isDescending)
    {
        // Arrange
        const int sampleAmount = 100;

        var context = new OrderContext
        {
            IsDescending = isDescending,
            PropertyName = orderingBy
        };

        List<SampleOrderingClass> samples = AutoFaker
            .Generate<SampleOrderingClass>(sampleAmount);

        IOrderedQueryable<SampleOrderingClass> expectedResult = MapExpectedResult(
            samples,
            orderingBy,
            isDescending);

        // Act
        Func<IOrderedQueryable<SampleOrderingClass>> function =
            () => _factory.OrderDynamically(samples.AsQueryable(), context);

        // Assert
        function
            .Should()
            .NotThrow()
            .Subject
            .Should()
            .BeEquivalentTo(expectedResult,
                            options => options.WithStrictOrdering());
    }

    private IOrderedQueryable<SampleOrderingClass> MapExpectedResult(
        List<SampleOrderingClass> samples,
        string orderingBy,
        bool isDescending)
    {
        return orderingBy switch
        {
            SampleOrderingClassConstants.OrderingBy.Number when isDescending =>
                samples.AsQueryable().OrderByDescending(sample => sample.Number),
            SampleOrderingClassConstants.OrderingBy.Number when !isDescending =>
                samples.AsQueryable().OrderBy(sample => sample.Number),

            SampleOrderingClassConstants.OrderingBy.NumberNested when isDescending =>
                samples.AsQueryable().OrderByDescending(sample => sample.Nested.Number),
            SampleOrderingClassConstants.OrderingBy.NumberNested when !isDescending =>
                samples.AsQueryable().OrderBy(sample => sample.Nested.Number),

            SampleOrderingClassConstants.OrderingBy.Pointed when isDescending =>
                samples.AsQueryable().OrderByDescending(sample => sample.Pointed),
            SampleOrderingClassConstants.OrderingBy.Pointed when !isDescending =>
                samples.AsQueryable().OrderBy(sample => sample.Pointed),

            SampleOrderingClassConstants.OrderingBy.PointedNested when isDescending =>
                samples.AsQueryable().OrderByDescending(sample => sample.Nested.Pointed),
            SampleOrderingClassConstants.OrderingBy.PointedNested when !isDescending =>
                samples.AsQueryable().OrderBy(sample => sample.Nested.Pointed),

            SampleOrderingClassConstants.OrderingBy.Textual when isDescending =>
                samples.AsQueryable().OrderByDescending(sample => sample.Textual),
            SampleOrderingClassConstants.OrderingBy.Textual when !isDescending =>
                samples.AsQueryable().OrderBy(sample => sample.Textual),

            SampleOrderingClassConstants.OrderingBy.TextualNested when isDescending =>
                samples.AsQueryable().OrderByDescending(sample => sample.Nested.Textual),
            SampleOrderingClassConstants.OrderingBy.TextualNested when !isDescending =>
                samples.AsQueryable().OrderBy(sample => sample.Nested.Textual),

            SampleOrderingClassConstants.OrderingBy.Precised when isDescending =>
                samples.AsQueryable().OrderByDescending(sample => sample.Precised),
            SampleOrderingClassConstants.OrderingBy.Precised when !isDescending =>
                samples.AsQueryable().OrderBy(sample => sample.Precised),

            SampleOrderingClassConstants.OrderingBy.PrecisedNested when isDescending =>
                samples.AsQueryable().OrderByDescending(sample => sample.Nested.Precised),
            SampleOrderingClassConstants.OrderingBy.PrecisedNested when !isDescending =>
                samples.AsQueryable().OrderBy(sample => sample.Nested.Precised),

            _ => throw new ArgumentOutOfRangeException(nameof(orderingBy))
        };
    }
}
