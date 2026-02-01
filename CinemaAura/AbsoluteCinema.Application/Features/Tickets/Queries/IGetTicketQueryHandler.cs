using AbsoluteCinema.Application.DTOs.Ticket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbsoluteCinema.Application.Features.Tickets.Queries
{
    public interface IGetTicketQueryHandler
    {
        Task<GetTicketDetailsResponse?> ExecuteAsync(GetTicketQuery query, CancellationToken ct);
    }
}
