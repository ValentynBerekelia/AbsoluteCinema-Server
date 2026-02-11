using System.ComponentModel.DataAnnotations;

namespace AbsoluteCinema.Requests;

public record UsersWithTicketsCountParameters(
    [Range(1, int.MaxValue)] int PageNumber = 1,
    [Range(1, int.MaxValue)] int PageSize = 10,
    string? SearchTerm = null
);