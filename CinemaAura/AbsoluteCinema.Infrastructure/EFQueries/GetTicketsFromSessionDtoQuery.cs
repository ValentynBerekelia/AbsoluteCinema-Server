using AbsoluteCinema.Application.DTOs.Ticket;
using AbsoluteCinema.Application.Features.Sessions.Queries;
using AbsoluteCinema.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace AbsoluteCinema.Infrastructure.EFQueries
{
    public class GetTicketsFromSessionDtoQuery(CinemaDbContext db) : IGetTicketsFromSessionDtoQuery
    {
        private readonly CinemaDbContext _db = db;

        public async Task<List<GetTicketDetailsResponse>?> ExecuteAsync(GetTicketsFromSessionQuery query,CancellationToken ct)
        {
            return await _db.Tickets
                .AsNoTracking()
                .Where(t => t.SessionId == query.SessionId)
                .Select(t => new GetTicketDetailsResponse(
                    t.Id,

                    t.UserId == null
                        ? null
                        : new UserShortDto(t.User!.Id, t.User.Email),

                    new SessionForTicketDto(
                        t.Session.Id,
                        t.Session.StartDateTime,
                        new HallForTicketsDto(
                            t.Session.Hall.Id,
                            t.Session.Hall.HallName
                        )
                    ),

                    new SeatForTicketDto(
                        t.Seat.Id,
                        t.Seat.Row,
                        t.Seat.Number,
                        new SeatTypeForTicketDto(
                            t.Seat.SeatTypeId,
                            t.Seat.SeatType.TypeName
                        )
                    ),

                    _db.TypePrices
                        .Where(tp => tp.SessionId == t.SessionId && tp.SeatTypeId == t.Seat.SeatTypeId)
                        .Select(tp => tp.Price)
                        .FirstOrDefault()
                ))
                .ToListAsync(ct);
        }
    }
}
