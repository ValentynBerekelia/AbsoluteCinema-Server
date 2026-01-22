using AbsoluteCinema.Application.Repository;
using AbsoluteCinema.Domain.Entities;
using AbsoluteCinema.Infrastructure.Persistence;

namespace AbsoluteCinema.Infrastructure.Repositories;

public class TicketRepository : BaseRepository<TicketId, Ticket, CinemaDbContext>, ITicketRepository
{
    public TicketRepository(CinemaDbContext dbContext) : base(dbContext)
    {
    }
}
