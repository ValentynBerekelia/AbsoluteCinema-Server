using AbsoluteCinema.Domain.Entities;

namespace AbsoluteCinema.Domain.Interfaces;

public interface ISessionRepository
{
    Task AddSessionAsync(Session session, CancellationToken cancellationToken = default);
    Task AddTypePriceAsync(TypePrice typePrice, CancellationToken cancellationToken = default);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
