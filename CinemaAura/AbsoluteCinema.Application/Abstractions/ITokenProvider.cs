using AbsoluteCinema.Domain.Entities;

namespace AbsoluteCinema.Application.Abstractions;

public interface ITokenProvider
{
    string GenerateAccessToken(User user);
    (string Token, string Hash) GenerateRefreshToken();
}

