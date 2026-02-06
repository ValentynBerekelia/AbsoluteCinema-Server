using AbsoluteCinema.Domain.Enums;

namespace AbsoluteCinema.Requests;

/// <summary>
/// Query parameters for searching persons (for autocomplete at FE)
/// </summary>
public record GetPersonsQueryParameters(
    string? Search = null,
    PersonRole? Role = null,
    int Limit = 20
);