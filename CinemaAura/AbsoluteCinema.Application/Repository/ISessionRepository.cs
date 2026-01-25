using AbsoluteCinema.Application.Abstractions;
using AbsoluteCinema.Domain.Entities;

namespace AbsoluteCinema.Application.Repository;

public interface ISessionRepository : IRepository<SessionId, Session>
{
    Task SaveChangesAsync(CancellationToken ct = default);

    Task<Session?> GetByIdWithPricesAsync(SessionId id, CancellationToken ct = default);

    Task<List<Session>> GetAllWithDetailsAsync(CancellationToken ct = default);
}