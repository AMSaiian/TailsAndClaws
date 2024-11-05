using AMSaiian.Shared.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TailsAndClaws.Application.Dogs.Constants;
using TailsAndClaws.Domain.Entities;
using TailsAndClaws.Infrastructure.Persistence.Constraints;

namespace TailsAndClaws.Infrastructure.Persistence.Configurations;

public class DogConfiguration : BaseEntityConfiguration<Dog>
{
    public override void Configure(EntityTypeBuilder<Dog> builder)
    {
        base.Configure(builder);

        builder.ToTable(table =>
        {
            table.AddGreaterThanZeroCheckConstraint("dogs_tail_length_in_meters", "tail_length_in_meters");
            table.AddGreaterThanZeroCheckConstraint("dogs_weight_in_kg", "weight_in_kg");
        });

        builder
            .Property(dog => dog.Name)
            .HasMaxLength(ValidationConstants.NameLength)
            .IsRequired();

        builder
            .Property(dog => dog.NormalizedName)
            .HasMaxLength(ValidationConstants.NameLength)
            .HasComputedColumnSql(string.Format(
                                      DataSchemeConstraints.UpperFunctionForColumnFormat,
                                      "name"),
                                  stored: true)
            .IsRequired();

        builder
            .HasIndex(dog => dog.NormalizedName)
            .IsUnique();

        builder
            .Property(dog => dog.Color)
            .HasMaxLength(ValidationConstants.ColorLength)
            .IsRequired();

        builder
            .Property(dog => dog.WeightInKg)
            .HasPrecision(
                ValidationConstants.WeightPrecision.Precision,
                ValidationConstants.WeightPrecision.Scale)
            .IsRequired();

        builder
            .Property(dog => dog.TailLengthInMeters)
            .HasPrecision(
                ValidationConstants.LengthPrecision.Precision,
                ValidationConstants.LengthPrecision.Scale)
            .IsRequired();
    }
}
