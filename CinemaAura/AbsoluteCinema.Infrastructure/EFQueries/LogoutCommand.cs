using AbsoluteCinema.Application.Abstractions;
using AbsoluteCinema.Application.Features.Auth.Command.Logout;
using AbsoluteCinema.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace AbsoluteCinema.Infrastructure.EFQueries;

public class LogoutCommandHandler(
    CinemaDbContext db,
    ITokenProvider tokenProvider,
    IRequestContext requestContext) : ILogoutCommand
{
    private readonly CinemaDbContext _db = db;
    private readonly ITokenProvider _tokenProvider = tokenProvider;
    private readonly IRequestContext _requestContext = requestContext;

    public async Task ExecuteAsync(LogoutCommand command, CancellationToken ct)
    {
        string tokenHash;
        try
        {
            tokenHash = _tokenProvider.HashRefreshToken(command.RefreshToken);
        }
        catch (FormatException)
        {
            return; // Invalid token format - nothing to revoke
        }

        var refreshToken = await _db.RefreshTokens
            .FirstOrDefaultAsync(rt => rt.TokenHash == tokenHash, ct);

        if (refreshToken == null || refreshToken.IsRevoked)
        {
            return; // Already revoked or not found
        }

        refreshToken.Revoke(_requestContext.IpAddress);
        await _db.SaveChangesAsync(ct);
    }
}
