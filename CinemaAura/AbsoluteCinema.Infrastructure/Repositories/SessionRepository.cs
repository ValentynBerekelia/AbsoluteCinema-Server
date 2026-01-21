using AbsoluteCinema.Application.Repository;
using AbsoluteCinema.Domain.Entities;
using AbsoluteCinema.Infrastructure.Persistence;

namespace AbsoluteCinema.Infrastructure.Repositories;

public class SessionRepository : BaseRepository<SessionId, Session, CinemaDbContext>, ISessionRepository
{
    public SessionRepository(CinemaDbContext dbContext) : base(dbContext)
    {
    }
}
