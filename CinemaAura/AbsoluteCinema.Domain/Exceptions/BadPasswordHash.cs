namespace AbsoluteCinema.Domain.Exceptions;

public class BadPasswordHashException : DomainException
{
    public BadPasswordHashException(string message) : base(message)
    {
    }
}