using AbsoluteCinema.Application.DTOs.Ticket;
using AbsoluteCinema.Application.EFQueries;
using AbsoluteCinema.Application.Features.Tickets.Queries;
using AbsoluteCinema.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using static AbsoluteCinema.Application.Features.Tickets.Queries.GetTicketsByUserQueryHandler;

namespace AbsoluteCinema.Infrastructure.EFQueries;

public sealed class GetTicketsByUserDtoQuery(CinemaDbContext db) : IGetTicketsByUserDtoQuery
{
    private readonly CinemaDbContext _db = db;

    public async Task<List<GetTicketForUserResponse>?> ExecuteAsync(GetTicketsByUserQuery query, CancellationToken ct)
    {
        return await _db.Tickets
            .AsNoTracking()
            .Where(t => t.UserId == query.UserId)
            .OrderByDescending(t => t.Session.StartDateTime)
            .Select(t => new GetTicketForUserResponse(
                t.Id.Id,  
                t.Status.ToString(),

                _db.TypePrices
                    .Where(tp => tp.SessionId == t.SessionId && tp.SeatTypeId == t.Seat.SeatTypeId)
                    .Select(tp => tp.Price)
                    .FirstOrDefault(),

                new UserSessionDto(
                    t.Session.Id.Id,      
                    t.Session.StartDateTime,
                    new HallDto(
                        t.Session.Hall.Id.Id,   
                        t.Session.Hall.HallName
                    )
                ),

                new SeatPositionDto(
                    t.Seat.Row,
                    t.Seat.Number
                ),

                new SeatTypeDto(
                    t.Seat.SeatTypeId.Id,
                    t.Seat.SeatType.TypeName
                ),

                new MovieDto(
                    t.Session.Movie.Id.Id,
                    t.Session.Movie.Name
                )
            ))
            .ToListAsync(ct);
    }
}
