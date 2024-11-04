using AMSaiian.Shared.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TailsAndClaws.Application.Dogs.Constants;
using TailsAndClaws.Domain.Entities;

namespace TailsAndClaws.Infrastructure.Persistence.Configurations;

public class DogConfiguration : BaseEntityConfiguration<Dog>
{
    public override void Configure(EntityTypeBuilder<Dog> builder)
    {
        base.Configure(builder);

        builder.ToTable(table =>
        {
            table.AddGreaterThanZeroCheckConstraint("dogs_tail_length", "tail_length");
        });

        builder
            .Property(dog => dog.Name)
            .HasMaxLength(ValidationConstants.NameLength)
            .IsRequired();

        builder
            .HasIndex(dog => dog.Name)
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
