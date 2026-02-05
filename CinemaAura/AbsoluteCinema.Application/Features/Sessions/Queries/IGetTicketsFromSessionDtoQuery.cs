using AbsoluteCinema.Application.DTOs.Ticket;

namespace AbsoluteCinema.Application.Features.Sessions.Queries
{
    public interface IGetTicketsFromSessionDtoQuery
    {
        Task<List<GetTicketDetailsResponse>?> ExecuteAsync(GetTicketsFromSessionQuery query, CancellationToken ct);
    }
}
