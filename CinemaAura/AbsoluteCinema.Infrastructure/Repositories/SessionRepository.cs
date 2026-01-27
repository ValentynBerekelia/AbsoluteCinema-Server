using AbsoluteCinema.Application.Repository;
using AbsoluteCinema.Domain.Entities;
using AbsoluteCinema.Infrastructure.Persistence;

namespace AbsoluteCinema.Infrastructure.Repositories;

public class SessionRepository : BaseRepository<SessionId, Session, CinemaDbContext>, ISessionRepository
{
    public SessionRepository(CinemaDbContext dbContext) : base(dbContext)
    {
    }

    public Task AddTypePriceAsync(TypePrice typePrice, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    public Task SaveChangesAsync(CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }
}
