using AbsoluteCinema.Application.DTOs.Ticket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbsoluteCinema.Application.Features.Tickets.Queries
{
    public interface IGetTicketShortQuery
    {
        Task<List<TicketShortDto>?> ExecuteAsync(GetTicketsQuery query,CancellationToken ct);
    }
}
