public class BaseRepository<TKey, TEntity, TContext> : 
    IRepository<TKey, TEntity>
    where TKey : notnull
    where TEntity : Entity<TKey> 
    where TContext : DbContext
{
    protected readonly TContext _dbContext;
    protected readonly DbSet<TEntity> _set;

    public BaseRepository(TContext dbContext)
    {
        _dbContext = dbContext;
        _set = _dbContext.Set<TEntity>();
    }
    
    public async Task<TEntity?> GetByIdAsync(TKey id, CancellationToken ct = default)
    {
        return await _set.AsNoTracking()
            .FirstOrDefaultAsync(e => EqualityComparer<TKey>.Default.Equals(e.Id, id), ct);
    }

    public async Task<TEntity?> GetByIdForUpdateAsync(TKey id, CancellationToken ct = default)
    {
        return await _set.FindAsync(new object?[] { id }, ct);
    }
    
    public async Task<List<TEntity>> GetAllBySpecificationAsync(Specification<TEntity> spec, CancellationToken ct = default)
    {
        return await ApplySpecification(spec).AsNoTracking().ToListAsync(ct);
    }

    public async Task<TEntity?> GetBySpecificationAsync(Specification<TEntity> spec, CancellationToken ct = default)
    {
        return await ApplySpecification(spec).AsNoTracking().FirstOrDefaultAsync(ct);
    }


    public async Task<List<TEntity>> GetAllAsync(CancellationToken ct = default)
    {
        return await _set.AsNoTracking().ToListAsync(ct);
    }
    
    public async Task<bool> AnyAsync(TKey id, CancellationToken ct = default)
    {
        return await _set.AnyAsync(e => EqualityComparer<TKey>.Default.Equals(e.Id, id), ct);
    }


    public Task AddAsync(TEntity entity, CancellationToken ct = default)
    {
        return _set.AddAsync(entity, ct).AsTask();
    }

    
    public void Update(TEntity entity)
    {
        _set.Update(entity);
    }
    
    public async Task DeleteAsync(TKey id, CancellationToken ct = default)
    {
        var entity = await _set.FindAsync(new object?[] { id }, ct);
        if (entity is null) return;
        _set.Remove(entity);
    }

    public Task<int> CountAsync(CancellationToken ct = default)
    {
        return _set.CountAsync(cancellationToken: ct);
    }

    private IQueryable<TEntity> ApplySpecification(Specification<TEntity> specification)
    {
        return SpecificationEvaluator<TKey,TEntity>.GetQuery(_set, specification);
    }
}
