using System.Security.Claims;

namespace AbsoluteCinema.Application.Abstractions;

public interface ITokenProvider
{
    string GenerateAccessToken(IEnumerable<Claim> claims);

    (string token, string tokenHash) GenerateRefreshToken();
    string HashRefreshToken(string refreshToken);
}
