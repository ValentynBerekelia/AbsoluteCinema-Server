using AbsoluteCinema.Domain.Specifications;

namespace AbsoluteCinema.Application.Abstructions;

public interface IRepository<in TKey, TEntity>
{
    //READ METHODS
    Task<TEntity?> GetByIdAsync(TKey id, CancellationToken ct = default);
    Task<TEntity?> GetByIdForUpdateAsync(TKey id, CancellationToken ct = default);
    Task<List<TEntity>> GetAllBySpecificationAsync(Specification<TEntity> spec, CancellationToken ct = default);
    Task<TEntity?> GetBySpecificationAsync(Specification<TEntity> spec, CancellationToken ct = default);
    Task<List<TEntity>> GetAllAsync( CancellationToken ct = default);
    Task<bool> AnyAsync(TKey id, CancellationToken ct = default);
    //WRITE METHODS
    Task AddAsync(TEntity entity, CancellationToken ct = default);
    void Update(TEntity entity);
    Task DeleteAsync(TKey id, CancellationToken ct = default);
    Task<int> CountAsync(CancellationToken ct = default);
}