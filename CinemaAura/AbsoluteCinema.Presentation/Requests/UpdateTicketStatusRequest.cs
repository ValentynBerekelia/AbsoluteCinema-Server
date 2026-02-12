using AbsoluteCinema.Domain.Entities;

namespace AbsoluteCinema.Requests;

public record UpdateTicketStatusRequest (
    TicketStatus Status
);