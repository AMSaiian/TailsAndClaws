using Microsoft.EntityFrameworkCore;
using TailsAndClaws.Domain.Entities;

namespace TailsAndClaws.Application.Common.Interfaces;

public interface IAppDbContext : IDisposable, IAsyncDisposable
{
    public DbSet<Dog> Dogs { get; set; }

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
