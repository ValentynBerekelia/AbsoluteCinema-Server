using AbsoluteCinema.Application.Repository;
using Microsoft.EntityFrameworkCore;

namespace AbsoluteCinema.Infrastructure.Repositories;

public class EfUnitOfWork<TContext> : IUnitOfWork where TContext : DbContext
{
    private readonly TContext _dbContext;

    public EfUnitOfWork(TContext dbContext) => _dbContext = dbContext;
    
    public Task<int> SaveChangesAsync(CancellationToken ct = default)
        => _dbContext.SaveChangesAsync(ct);

    public async Task ExecuteInTransactionAsync(Func<CancellationToken, Task> action, CancellationToken ct = default)
    {
        if (_dbContext.Database.CurrentTransaction is not null)
        {
            await action(ct);
            return;
        }

        await using var tx = await _dbContext.Database.BeginTransactionAsync(ct);
        try
        {
            await action(ct);
            await _dbContext.SaveChangesAsync(ct);
            await tx.CommitAsync(ct);
        }
        catch
        {
            await tx.RollbackAsync(ct);
            throw;
        }
    }
}