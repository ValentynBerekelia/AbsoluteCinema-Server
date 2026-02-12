using AbsoluteCinema.Application.Repository;
using AbsoluteCinema.Domain.Entities;
using AbsoluteCinema.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace AbsoluteCinema.Infrastructure.Repositories;

public class TicketRepository : BaseRepository<TicketId, Ticket, CinemaDbContext>, ITicketRepository
{
    public TicketRepository(CinemaDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<Ticket?> GetByIdWithDetailsAsync(TicketId id, CancellationToken ct = default)
    {
        return await _set
            .AsNoTracking()
            .Include(t => t.User)
            .Include(t => t.Seat)
            .Include(t => t.Session)
                .ThenInclude(s => s.Movie)
            .Include(t => t.Session)
                .ThenInclude(s => s.Hall)
            .FirstOrDefaultAsync(t => t.Id == id, ct);
    }
}