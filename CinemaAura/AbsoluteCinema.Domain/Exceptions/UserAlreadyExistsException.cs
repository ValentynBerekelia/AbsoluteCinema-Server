namespace AbsoluteCinema.Domain.Exceptions;

public class UserAlreadyExistsException : DomainException
{
    public UserAlreadyExistsException(string message) : base(message)
    {
    }
}