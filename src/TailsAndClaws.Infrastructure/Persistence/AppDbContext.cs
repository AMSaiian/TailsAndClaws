using AMSaiian.Shared.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
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

        modelBuilder.ApplyConfigurationsFromAssembly(InfrastructureAssemblyType.Reference);
        modelBuilder.ToSnakeCaseConventions();
    }
}

public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
        optionsBuilder.UseNpgsql();

        return new AppDbContext(optionsBuilder.Options);
    }
}
