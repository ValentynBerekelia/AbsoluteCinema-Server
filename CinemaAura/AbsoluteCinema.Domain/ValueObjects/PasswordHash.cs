using System.Text;
using AbsoluteCinema.Domain.Exceptions;

namespace AbsoluteCinema.Domain.ValueObjects;

public class PasswordHash
{
    public byte[] Value { get; init; }
    public byte[] Salt { get; init; }

    private PasswordHash() { }

    private PasswordHash(byte[] value, byte[] salt)
    {
        Value = value;
        Salt = salt;
    }

    // made this method STATIC
    public static PasswordHash Create(byte[] value, byte[] salt)
    {
        if (value.Length == 0 || salt.Length == 0)
        {
            throw new DomainException("Hash and Salt cannot be empty.");
        }

        return new PasswordHash(value, salt);
    }

    // added this Overload for Seeder (String -> Byte[])
    // allows PasswordHash.Create("password123") to work
    public static PasswordHash Create(string password)
    {
        if (string.IsNullOrWhiteSpace(password))
        {
            throw new DomainException("Password cannot be empty.");
        }

        // For seeding/testing only: 
        // We convert the string directly to bytes to simulate a hash and salt.
        // In a real registration flow, we would use a Hashing Service (like BCrypt) 
        // to generate these bytes properly.
        var dummyBytes = Encoding.UTF8.GetBytes(password);
        var dummySalt = Encoding.UTF8.GetBytes("salt_" + password); // fake salt

        return new PasswordHash(dummyBytes, dummySalt);
    }
}