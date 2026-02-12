namespace AbsoluteCinema.Application.DTOs.User;

public record UserWithTicketsCountDto (
    Guid Id,
    string UserName,
    string Email,
    int TotalTickets
);