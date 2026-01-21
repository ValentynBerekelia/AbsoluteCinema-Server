using AbsoluteCinema.Domain.Exceptions;

namespace AbsoluteCinema.Domain.ValueObjects;

public class PasswordHash
{
    public byte[] Value { get; init; }
    public byte[] Salt { get; init; }

    private PasswordHash() {}
    private PasswordHash(byte[] value, byte[] salt)
    {
        Value = value;
        Salt = salt;
    }

    public PasswordHash Create(byte[] value, byte[] salt)
    {
        if (value.Length == 0 || salt.Length == 0)
        {
            throw new BadPasswordHashException("hash.Length == 0 || salt.Length == 0");
        }

        return new PasswordHash(value, salt);
    }
}