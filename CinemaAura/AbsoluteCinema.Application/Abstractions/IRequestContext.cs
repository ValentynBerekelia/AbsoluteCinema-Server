namespace AbsoluteCinema.Application.Abstractions;

public interface IRequestContext
{
    string IpAddress { get; }
    string? UserAgent { get; }
    Guid? UserId { get; }
}