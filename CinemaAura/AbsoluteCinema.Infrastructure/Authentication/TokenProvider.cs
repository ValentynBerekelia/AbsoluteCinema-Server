using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using AbsoluteCinema.Application.Abstractions;
using AbsoluteCinema.Infrastructure.Security;
using Microsoft.IdentityModel.Tokens;

namespace AbsoluteCinema.Infrastructure.Authentication;

public class TokenProvider : ITokenProvider
{
    private readonly JwtOptions _jwtOptions;

    public TokenProvider(JwtOptions jwtOptions)
    {
        _jwtOptions = jwtOptions;
    }

    public string GenerateAccessToken(IEnumerable<Claim> claims)
    {
        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_jwtOptions.Key));

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _jwtOptions.Issuer,
            audience: _jwtOptions.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(15),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public (string token, string tokenHash) GenerateRefreshToken()
    {
        var bytes = RandomNumberGenerator.GetBytes(64);
        var token = Convert.ToBase64String(bytes);
        return (token, HashRefreshToken(token));
    }

    public string HashRefreshToken(string refreshToken)
    {
        using var sha = SHA256.Create();
        var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(refreshToken));
        return Convert.ToBase64String(bytes);
    }
}
