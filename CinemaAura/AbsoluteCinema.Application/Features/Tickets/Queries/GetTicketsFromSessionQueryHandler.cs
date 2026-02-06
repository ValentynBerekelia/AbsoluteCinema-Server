using AbsoluteCinema.Application.DTOs.Ticket;
using AbsoluteCinema.Domain.Entities;
using MediatR;

namespace AbsoluteCinema.Application.Features.Tickets.Queries
{
    public class GetTicketsFromSessionQueryHandler(IGetTicketsFromSessionDtoQuery query)
        : IRequestHandler<GetTicketsFromSessionQuery, List<GetTicketDetailsResponse>>
    {
        public async Task<List<GetTicketDetailsResponse>> Handle(GetTicketsFromSessionQuery request,CancellationToken ct)
        {
            var tickets = await query.ExecuteAsync(request, ct);
            return tickets ?? new List<GetTicketDetailsResponse>();
        }
    }

    public record GetTicketsFromSessionQuery : IRequest<List<GetTicketDetailsResponse>>
    {
        public SessionId SessionId { get; }
        public GetTicketsFromSessionQuery(SessionId id) => SessionId = id;
    }
}
