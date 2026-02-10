using AbsoluteCinema.Application.Abstractions;
using AbsoluteCinema.Application.Features.Auth.Command.RefreshToken;
using AbsoluteCinema.Domain.Entities;
using AbsoluteCinema.Domain.Exceptions;
using AbsoluteCinema.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace AbsoluteCinema.Infrastructure.EFQueries;

public class RefreshTokenCommand(
    CinemaDbContext db,
    ITokenProvider tokenProvider,
    IRequestContext requestContext) : IRefreshTokenCommand
{
    private readonly CinemaDbContext _db = db;
    private readonly ITokenProvider _tokenProvider = tokenProvider;
    private readonly IRequestContext _requestContext = requestContext;

    public async Task<RefreshTokenResponse> ExecuteAsync(Application.Features.Auth.Command.RefreshToken.RefreshTokenCommand command, CancellationToken ct)
    {
        var tokenHash = _tokenProvider.HashRefreshToken(command.RefreshToken);

        var refreshToken = await _db.RefreshTokens
            .Include(rt => rt.User)
                .ThenInclude(u => u.Roles)
                    .ThenInclude(r => r.Permissions)
            .FirstOrDefaultAsync(rt => rt.TokenHash == tokenHash, ct);

        if (refreshToken is null)
            throw new DomainException("Invalid refresh token.");

        if (refreshToken.IsRevoked)
            throw new DomainException("Refresh token has been revoked.");

        if (refreshToken.ExpiresAt < DateTime.UtcNow)
            throw new DomainException("Refresh token has expired.");

        var user = refreshToken.User;

        refreshToken.Revoke(_requestContext.IpAddress);

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Name, user.UserName),
        };

        foreach (var role in user.Roles.Select(r => r.Name).Distinct(StringComparer.OrdinalIgnoreCase))
            claims.Add(new Claim(ClaimTypes.Role, role));

        foreach (var perm in user.Roles
                     .SelectMany(r => r.Permissions)
                     .Select(p => p.Code.Value)
                     .Distinct(StringComparer.OrdinalIgnoreCase))
        {
            claims.Add(new Claim("perm", perm));
        }

        var accessToken = _tokenProvider.GenerateAccessToken(claims);

        var (newRefreshToken, newRefreshTokenHash) = _tokenProvider.GenerateRefreshToken();

        var newRt = RefreshToken.Create(
            user.Id,
            newRefreshTokenHash,
            expiresAt: DateTime.UtcNow.AddDays(14),
            createdByIp: _requestContext.IpAddress
        );

        _db.RefreshTokens.Add(newRt);
        await _db.SaveChangesAsync(ct);

        return new RefreshTokenResponse(user.Id.Id, accessToken, newRefreshToken);
    }
}
