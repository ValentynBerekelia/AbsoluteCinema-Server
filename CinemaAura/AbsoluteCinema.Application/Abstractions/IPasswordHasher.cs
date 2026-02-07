using AbsoluteCinema.Domain.ValueObjects;

namespace AbsoluteCinema.Application.Abstractions;

public interface IPasswordHasher
{

    PasswordHash Hash(string password);

    bool Verify(string password, PasswordHash passwordHash);
}