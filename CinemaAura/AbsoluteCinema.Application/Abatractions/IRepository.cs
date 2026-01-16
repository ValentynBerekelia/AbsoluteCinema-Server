using CinemaAura.Domain.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaAura.Application.Abatractions
{
    public interface IRepository<in TKey, TEntity>
    {
        Task<TEntity?> GetByIdAsync(TKey id);
        Task<List<TEntity>> GetAllBySpecificationAsync(Specification<TEntity> spec);
        Task<TEntity?> GetBySpecificationAsync(Specification<TEntity> spec);
        Task<List<TEntity>> GetAllAsync();
        Task<TEntity> CreateAsync(TEntity entity);
        Task<bool> AnyAsync(TKey id);
        Task UpdateAsync(TEntity entity);
        Task DeleteAsync(TKey id);
        Task<int> CountAsync();
    }
}
