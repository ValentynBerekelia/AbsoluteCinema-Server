using AbsoluteCinema.Domain.Entities;

namespace AbsoluteCinema.Application.DTOs.Ticket;

public record UpdateTicketStatusDto (
    Guid Id,
    TicketStatus Status,
    Guid UserId,
    bool? IsAdmin
);