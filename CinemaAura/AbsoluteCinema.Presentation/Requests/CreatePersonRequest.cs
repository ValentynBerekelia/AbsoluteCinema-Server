using AbsoluteCinema.Domain.Enums;

namespace AbsoluteCinema.Requests
{
    /// <summary>
    /// Request for creating new person and attaching to movie
    /// </summary>
    public record CreatePersonRequest(
        string FullName,
        string? Bio,
        DateTime BirthDate,
        PersonRole Role
    );
}
