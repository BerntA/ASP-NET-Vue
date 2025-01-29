using database;
using Microsoft.EntityFrameworkCore;

namespace lib.Services;

public abstract class RepositoryService : IDisposable, IAsyncDisposable
{
    protected readonly ApplicationDbContext _dbContext;

    public RepositoryService(IDbContextFactory<ApplicationDbContext> dbContextFactory) =>
        _dbContext = dbContextFactory.CreateDbContext();

    public void Dispose() => _dbContext.Dispose();

    public ValueTask DisposeAsync() => _dbContext.DisposeAsync();
}