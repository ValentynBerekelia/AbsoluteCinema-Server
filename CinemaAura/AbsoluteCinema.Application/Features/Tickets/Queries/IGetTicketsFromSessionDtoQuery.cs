using AbsoluteCinema.Application.DTOs.Ticket;

namespace AbsoluteCinema.Application.Features.Tickets.Queries
{
    public interface IGetTicketsFromSessionDtoQuery
    {
        Task<List<GetTicketDetailsResponse>?> ExecuteAsync(GetTicketsFromSessionQuery query, CancellationToken ct);
    }
}
