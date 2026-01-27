using AbsoluteCinema.Application.Abstractions;
using AbsoluteCinema.Domain.Entities;

namespace AbsoluteCinema.Application.Repository;

public interface IHallRepository : IRepository<HallId, Hall>
{
    Task<Hall?> GetByNameAsync(string name, CancellationToken ct = default);
}
