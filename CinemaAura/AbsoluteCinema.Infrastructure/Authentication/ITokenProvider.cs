using AbsoluteCinema.Domain.Entities;

namespace AbsoluteCinema.Infrastructure.Security;

public interface ITokenProvider
{
    string GenerateAccessToken(User user);
    string GenerateRefreshToken();
    string HashRefreshToken(string token);
}