using System.Reflection;
using AMSaiian.Shared.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;
using TailsAndClaws.Application.Common.Interfaces;
using TailsAndClaws.Domain.Entities;
using TailsAndClaws.Infrastructure.Persistence.Constraints;

namespace TailsAndClaws.Infrastructure.Persistence;

public class AppDbContext(DbContextOptions<AppDbContext> options)
    : DbContext(options),
      IAppDbContext
{
    public DbSet<Dog> Dogs { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.HasPostgresExtension(DataSchemeConstraints.KeyGenerationExtensionName);
        modelBuilder.HasDefaultSchema(DataSchemeConstraints.SchemeName);

        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        modelBuilder.ToSnakeCaseConventions();
    }
}
