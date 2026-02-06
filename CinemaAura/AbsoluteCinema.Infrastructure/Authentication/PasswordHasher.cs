using System.Security.Cryptography;
using AbsoluteCinema.Application.Abstractions;
using AbsoluteCinema.Domain.ValueObjects;

namespace AbsoluteCinema.Infrastructure.Security;

public sealed class PasswordHasher : IPasswordHasher
{
    private const int SaltSize = 16; 
    private const int KeySize = 32; 
    private const int Iterations = 100_000;
    private static readonly HashAlgorithmName Algorithm = HashAlgorithmName.SHA256;

    public PasswordHash Hash(string password)
    {
        if (string.IsNullOrWhiteSpace(password))
        {
            throw new ArgumentException("Password cannot be null or empty.", nameof(password));
        }

        var salt = RandomNumberGenerator.GetBytes(SaltSize);
        var hash = Rfc2898DeriveBytes.Pbkdf2(
            password,
            salt,
            Iterations,
            Algorithm,
            KeySize);

        return PasswordHash.Create(hash, salt);
    }

    public bool Verify(string password, PasswordHash passwordHash)
    {
        if (passwordHash is null)
        {
            return false;
        }

        var hashToCompare = Rfc2898DeriveBytes.Pbkdf2(
            password,
            passwordHash.Salt,
            Iterations,
            Algorithm,
            passwordHash.Value.Length);

        return CryptographicOperations.FixedTimeEquals(hashToCompare, passwordHash.Value);
    }
}