using Bogus;
using Bogus.Extensions;
using TailsAndClaws.Application.Dogs.Constants;
using TailsAndClaws.Domain.Entities;

namespace TailsAndClaws.Infrastructure.Persistence.Seeding.Fakers;

public sealed class DogFaker : Faker<Dog>
{
    public DogFaker()
    {
        RuleFor(dog => dog.Name,
                faker => (faker.Name.FirstName() + Guid.NewGuid())
                    .ClampLength(
                        max: ValidationConstants.NameLength));

        RuleFor(dog => dog.Color,
                faker => faker.Lorem.Sentence()
                    .ClampLength(
                        max: ValidationConstants.ColorLength));

        RuleFor(dog => dog.WeightInKg,
                faker => faker.Random
                    .Decimal(ExpectedMinimalMeasureUnit,
                             ExpectedMaxDogWeight));

        RuleFor(dog => dog.TailLengthInMeters,
                faker => faker.Random
                    .Decimal(ExpectedMinimalMeasureUnit,
                             ExpectedMaxDogTailLength));
    }

    private const decimal ExpectedMaxDogWeight = 100;
    private const decimal ExpectedMaxDogTailLength = 3;
    private const decimal ExpectedMinimalMeasureUnit = 0.001M;
}
