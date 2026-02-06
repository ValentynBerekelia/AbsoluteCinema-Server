using AbsoluteCinema.Application.DTOs;
using AbsoluteCinema.Application.DTOs.Ticket;
using AbsoluteCinema.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbsoluteCinema.Application.Features.Tickets.Queries
{
    public class GetTicketShortQueryHandler(IGetTicketShortQuery query)
          : IRequestHandler<GetTicketsQuery, GetTicketsResponse>
    {
        public async Task<GetTicketsResponse> Handle(GetTicketsQuery request,CancellationToken ct)
        {
            var tickets = await query.ExecuteAsync(request, ct);

            if (tickets is null)
                throw new Exception($"Session {request.SessionId} not found");

            return new GetTicketsResponse(tickets);
        }
    }
    public record GetTicketsQuery(SessionId SessionId)
        : IRequest<GetTicketsResponse>;

    public record GetTicketsResponse(List<TicketShortDto> Tickets);
}
