using AbsoluteCinema.Application.DTOs.Ticket;
using AbsoluteCinema.Domain.Entities;
using MediatR;

namespace AbsoluteCinema.Application.Features.Tickets.Queries
{
    public class GetTicketQueryHandler(IGetTicketQueryHandler query)
        : IRequestHandler<GetTicketQuery, GetTicketDetailsResponse>
    {
        public async Task<GetTicketDetailsResponse> Handle(GetTicketQuery request, CancellationToken cancellationToken)
        {
            var ticket = await query.ExecuteAsync(request, cancellationToken);

            if (ticket is null)
                throw new Exception($"Ticket {request.TicketId} not found");

            return ticket;
        }
    }

    public record GetTicketQuery : IRequest<GetTicketDetailsResponse>
    {
        public TicketId TicketId { get; }
        public GetTicketQuery(TicketId id) => TicketId = id;
    }
}