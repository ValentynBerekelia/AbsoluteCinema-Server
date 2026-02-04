using System.Runtime.InteropServices;
using AbsoluteCinema.Domain.Primitives;

namespace AbsoluteCinema.Domain.Entities;

public class RefreshToken : Entity<RefreshTokenId>
{
    public User User { get; private set; }
    public UserId UserId { get; private set; }
    
    public string TokenHash { get; private set; } = null!;
    public DateTime ExpiresAt { get; private set; }

    public bool IsRevoked { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public string CreatedByIp { get; private set; } = null!;
    public DateTime? RevokedAt { get; private set; }
    public string? RevokedByIp { get; private set; }

    private RefreshToken(UserId userId, string tokenHash, DateTime expiresAt, string createdByIp)
    {
        Id = RefreshTokenId.New();
        UserId = userId;
        TokenHash = tokenHash;
        ExpiresAt = expiresAt;
        CreatedAt = DateTime.UtcNow;
        CreatedByIp = createdByIp;
        IsRevoked = false;
    }
    public static RefreshToken Create(UserId userId, string tokenHash, DateTime expiresAt, string createdByIp)
    {
        return new RefreshToken(userId, tokenHash, expiresAt, createdByIp);
    }

    public void Revoke(string revokedByIp)
    {
        IsRevoked = true;
        RevokedAt = DateTime.UtcNow;
        RevokedByIp = revokedByIp;
    }
}

public record RefreshTokenId(Guid Id)
{
    public static RefreshTokenId New() => new RefreshTokenId(Guid.NewGuid());
}