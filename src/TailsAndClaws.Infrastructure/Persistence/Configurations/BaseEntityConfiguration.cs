using AMSaiian.Shared.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TailsAndClaws.Infrastructure.Persistence.Constraints;

namespace TailsAndClaws.Infrastructure.Persistence.Configurations;

public abstract class BaseEntityConfiguration<TEntity> : IEntityTypeConfiguration<TEntity>
    where TEntity : BaseEntity
{
    public virtual void Configure(EntityTypeBuilder<TEntity> builder)
    {
        builder.HasKey(ent => ent.Id);

        builder
            .Property(ent => ent.Id)
            .HasColumnType(DataSchemeConstraints.KeyType)
            .HasDefaultValueSql(DataSchemeConstraints.KeyTypeDefaultValue);
    }
}
