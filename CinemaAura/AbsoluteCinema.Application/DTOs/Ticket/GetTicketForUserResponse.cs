namespace AbsoluteCinema.Application.DTOs.Ticket;

public sealed record GetTicketForUserResponse(
    Guid Id,
    string Status,
    decimal Price,
    UserSessionDto Session,
    SeatPositionDto Seat,
    SeatTypeDto SeatType,
    MovieDto Movie
);

public sealed record UserSessionDto(
    Guid Id,
    DateTime StartDateTime,
    HallDto Hall
);

public sealed record HallDto(
    Guid Id,
    string Name
);

public sealed record SeatPositionDto(
    short Row,
    short Number
);
public sealed record MovieDto(
    Guid Id,
    string Name
);

public sealed record SeatTypeDto(
    Guid Id,
    string Name
);