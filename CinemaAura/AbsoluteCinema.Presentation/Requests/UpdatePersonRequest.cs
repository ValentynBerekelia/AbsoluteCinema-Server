using AbsoluteCinema.Domain.Enums;

namespace AbsoluteCinema.Requests;

public record UpdatePersonRequest(
    string FullName,
    string? Bio,
    DateTime BirthDate,
    PersonRole Role
);