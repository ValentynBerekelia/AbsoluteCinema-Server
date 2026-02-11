using AbsoluteCinema.Application.DTOs.Ticket;
using AbsoluteCinema.Application.Features.Tickets.Queries;
using static AbsoluteCinema.Application.Features.Tickets.Queries.GetTicketsByUserQueryHandler;

namespace AbsoluteCinema.Application.EFQueries;

public interface IGetTicketsByUserDtoQuery
{
    Task<List<GetTicketForUserResponse>?> ExecuteAsync(GetTicketsByUserQuery query, CancellationToken ct);
}