namespace AbsoluteCinema.Application.DTOs;

public record AdminMovieStatsDto (
    Guid Id,
    string Name,
    string PosterUrl,
    TimeSpan Duration,
    int AgeLimit,
    int TotalTicketSold,
    int TotalCapacity,
    List<SessionDto> Sessions
);