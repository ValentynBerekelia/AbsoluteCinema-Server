using AbsoluteCinema.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbsoluteCinema.Application.DTOs.Ticket
{
    public record TicketShortDto(
        TicketId TicketId,
        SeatId SeatId
        );
}
