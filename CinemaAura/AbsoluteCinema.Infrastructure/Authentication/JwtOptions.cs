namespace AbsoluteCinema.Infrastructure.Security;

public sealed class JwtOptions
{
    public string Key { get; init; } = null!;
    public string Issuer { get; init; } = null!;
    public string Audience { get; init; } = null!;
    public int AccessTokenMinutes { get; init; }
    public int RefreshTokenDays { get; init; }
    public string Algorithm { get; init; } = "HS256";
}