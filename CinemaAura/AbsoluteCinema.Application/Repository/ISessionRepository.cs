using AbsoluteCinema.Application.Abstractions;
using AbsoluteCinema.Domain.Entities;

namespace AbsoluteCinema.Application.Repository;

public interface ISessionRepository : IRepository<SessionId, Session>
{
    Task AddTypePriceAsync(TypePrice typePrice, CancellationToken ct = default);
    Task SaveChangesAsync(CancellationToken ct = default);
}
