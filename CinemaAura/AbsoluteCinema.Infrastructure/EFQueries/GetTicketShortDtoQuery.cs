using AbsoluteCinema.Application.DTOs.Ticket;
using AbsoluteCinema.Application.Features.Tickets.Queries;
using AbsoluteCinema.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace AbsoluteCinema.Infrastructure.EFQueries
{
    public class GetTicketShortDtoQuery(CinemaDbContext db) : IGetTicketShortQuery
    {
        private readonly CinemaDbContext _db = db;

        public async Task<List<TicketShortDto>?> ExecuteAsync(GetTicketsQuery query, CancellationToken ct)
        {
            var sessionExists = await _db.Sessions
                .AsNoTracking()
                .AnyAsync(s => s.Id == query.SessionId, ct);

            if (!sessionExists)
                return null;

            return await _db.Tickets
                .AsNoTracking()
                .Where(t => t.SessionId == query.SessionId)
                .Select(t => new TicketShortDto(
                    t.Id,
                    t.SeatId
                ))
                .ToListAsync(ct);
        }
    }
}
