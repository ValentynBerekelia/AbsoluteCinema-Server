using AbsoluteCinema.Application.Abstractions;
using AbsoluteCinema.Application.Common.Exceptions;
using AbsoluteCinema.Application.Features.Auth.Command.RefreshToken;
using AbsoluteCinema.Domain.Entities;
using AbsoluteCinema.Domain.Exceptions;
using AbsoluteCinema.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace AbsoluteCinema.Infrastructure.EFQueries;

public class RefreshTokenCommandHandler(
    CinemaDbContext db,
    ITokenProvider tokenProvider,
    IRequestContext requestContext) : IRefreshTokenCommand
{
    private readonly CinemaDbContext _db = db;
    private readonly ITokenProvider _tokenProvider = tokenProvider;
    private readonly IRequestContext _requestContext = requestContext;

    public async Task<RefreshTokenResponse> ExecuteAsync(RefreshTokenCommand command, CancellationToken ct)
    {
        var tokenHash = _tokenProvider.HashRefreshToken(command.RefreshToken);

        var refreshToken = await _db.RefreshTokens
            .Include(rt => rt.User)
            .FirstOrDefaultAsync(rt => rt.TokenHash == tokenHash, ct);

        if (refreshToken == null)
        {
            throw new DomainException("Invalid refresh token.");
        }

        if (refreshToken.IsRevoked)
        {
            throw new DomainException("Refresh token has been revoked.");
        }

        if (refreshToken.ExpiresAt < DateTime.UtcNow)
        {
            throw new DomainException("Refresh token has expired.");
        }

        var user = refreshToken.User;

        refreshToken.Revoke(_requestContext.IpAddress);

        var accessToken = _tokenProvider.GenerateAccessToken(user);
        var (newRefreshToken, newRefreshTokenHash) = _tokenProvider.GenerateRefreshToken();

        var newRt = RefreshToken.Create(
            user.Id,
            newRefreshTokenHash,
            expiresAt: DateTime.UtcNow.AddDays(14),
            createdByIp: _requestContext.IpAddress
        );

        _db.RefreshTokens.Add(newRt);
        await _db.SaveChangesAsync(ct);

        return new RefreshTokenResponse(
            user.Id.Id,
            accessToken,
            newRefreshToken
        );
    }
}
