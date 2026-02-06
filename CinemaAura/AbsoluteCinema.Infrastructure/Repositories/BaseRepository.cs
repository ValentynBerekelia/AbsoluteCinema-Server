using AbsoluteCinema.Application.Abstractions;
using AbsoluteCinema.Domain.Primitives;
using AbsoluteCinema.Domain.Specifications;
using AbsoluteCinema.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
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

    /// <summary>
    /// Get entity by ID (read-only, no tracking).
    /// Use for queries where you don't need to update the entity.
    /// </summary>
    public async Task<TEntity?> GetByIdAsync(TKey id, CancellationToken ct = default)
    {
        var entity = await _set.FindAsync(new object?[] { id }, ct);

        // detach for read-only scenario (no tracking)
        if (entity is not null)
            _dbContext.Entry(entity).State = EntityState.Detached;

        return entity;
    }

    /// <summary>
    /// Get entity by ID with change tracking enabled.
    /// Use for commands where you need to update/delete the entity.
    /// </summary>
    public async Task<TEntity?> GetByIdForUpdateAsync(TKey id, CancellationToken ct = default)
    {
        return await _set.FindAsync(new object?[] { id }, ct);
    }

    /// <summary>
    /// Get list of entities by specification.
    /// Tracking controlled by spec.AsNoTracking (default: true = no tracking).
    /// </summary>
    public async Task<List<TEntity>> GetAllBySpecificationAsync(Specification<TEntity> spec, CancellationToken ct = default)
    {
        // return await ApplySpecification(spec).AsNoTracking().ToListAsync(ct);

        var query = ApplySpecification(spec);

        if (spec.AsNoTracking)
            query = query.AsNoTracking();

        return await query.ToListAsync(ct);
    }

    /// <summary>
    /// Get single entity by specification.
    /// Tracking controlled by spec.AsNoTracking (default: true = no tracking).
    /// For update operations, use ApplyTracking(true) in specification.
    /// </summary>
    public async Task<TEntity?> GetBySpecificationAsync(Specification<TEntity> spec, CancellationToken ct = default)
    {
        // return await ApplySpecification(spec).AsNoTracking().FirstOrDefaultAsync(ct);

        var query = ApplySpecification(spec);

        if (spec.AsNoTracking)
            query = query.AsNoTracking();

        return await query.FirstOrDefaultAsync(ct);
    }

    /// <summary>
    /// Get all entities (read-only, no tracking).
    /// </summary>
    public async Task<List<TEntity>> GetAllAsync(CancellationToken ct = default)
    {
        return await _set.AsNoTracking().ToListAsync(ct);
    }

    /// <summary>
    /// Check if entity with given ID exists.
    /// No tracking is applied, only checks existence.
    /// </summary>
    public async Task<bool> AnyAsync(TKey id, CancellationToken ct = default)
    {
        var entity = await _set.FindAsync(new object?[] { id }, ct);

        if (entity is null)
            return false;

        // detach for read-only scenario / only check existence (no tracking)
        _dbContext.Entry(entity).State = EntityState.Detached;

        return true;
    }

    /// <summary>
    /// Add entity.
    /// </summary>
    public Task AddAsync(TEntity entity, CancellationToken ct = default)
    {
        return _set.AddAsync(entity, ct).AsTask();
    }

    /// <summary>
    /// Update existing entity.
    /// Entity must be tracked or will be attached as Modified.
    /// </summary>
    public void Update(TEntity entity)
    {
        _set.Update(entity);
    }

    /// <summary>
    /// Delete entity by ID.
    /// </summary>
    public async Task DeleteAsync(TKey id, CancellationToken ct = default)
    {
        var entity = await _set.FindAsync(new object?[] { id }, ct);
        if (entity is null) return;
        _set.Remove(entity);
    }

    /// <summary>
    /// Get count of all entities.
    /// </summary>
    public Task<int> CountAsync(CancellationToken ct = default)
    {
        return _set.CountAsync(cancellationToken: ct);
    }

    private IQueryable<TEntity> ApplySpecification(Specification<TEntity> specification)
    {
        return SpecificationEvaluator<TKey, TEntity>.GetQuery(_set, specification);
    }
}
