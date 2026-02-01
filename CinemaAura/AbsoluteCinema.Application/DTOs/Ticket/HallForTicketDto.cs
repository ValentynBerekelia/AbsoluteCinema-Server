using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AbsoluteCinema.Domain.Entities;

namespace AbsoluteCinema.Application.DTOs.Ticket
{
    public record HallForTicketDto(
        HallId Id,
        string HallName
    );

}
