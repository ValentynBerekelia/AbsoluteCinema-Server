namespace AbsoluteCinema.Requests;

public record MovieCreateRequest(
    string MovieName,
    string Description,
    decimal Rate,
    int AgeLimit,
    TimeSpan Duration,
    string Country,
    string Studio,
    string Language
);