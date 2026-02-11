using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AbsoluteCinema.Domain.Entities;

namespace AbsoluteCinema.Application.DTOs.Ticket
{
    public record GetTicketDetailsResponse(
        TicketId Id,
        string Status,
        UserShortDto? User,
        SessionForTicketDto Session,
        SeatForTicketDto Seat,
        decimal Price
    );

    public record UserShortDto(UserId Id, string Email);

    public record SessionForTicketDto(
        SessionId Id,
        DateTime StartDateTime,
        HallForTicketsDto Hall
    );

    public record HallForTicketsDto(HallId Id, string Name);

    public record SeatForTicketDto(
        SeatId Id,
        short Row,
        short Number,
        SeatTypeForTicketDto SeatType
    );

    public record SeatTypeForTicketDto(SeatTypeId Id, string Name);
}
