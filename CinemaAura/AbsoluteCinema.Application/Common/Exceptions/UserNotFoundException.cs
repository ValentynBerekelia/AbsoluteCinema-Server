namespace AbsoluteCinema.Application.Common.Exceptions;

public class UserNotFoundException : Exception
{
    public UserNotFoundException(string email) : base($"User with email {email} not found") 
    {
    }
}