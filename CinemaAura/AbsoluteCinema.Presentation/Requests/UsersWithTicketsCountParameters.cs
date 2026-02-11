namespace AbsoluteCinema.Presentation;

public record UsersWithTicketsCountParameters(
    int PageNumber = 1,
    int PageSize = 10,
    string? SearchTerm = null
);