using AbsoluteCinema.Application.Abstractions;
using AbsoluteCinema.Application.Features.Auth.Command.RevokeAllRefreshTokens;
using AbsoluteCinema.Domain.Entities;
using AbsoluteCinema.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace AbsoluteCinema.Infrastructure.EFQueries;

public class RevokeAllRefreshTokensCommandHandler(
    CinemaDbContext db,
    IRequestContext requestContext) : IRevokeAllRefreshTokensCommand
{
    private readonly CinemaDbContext _db = db;
    private readonly IRequestContext _requestContext = requestContext;

    public async Task ExecuteAsync(RevokeAllRefreshTokensCommand command, CancellationToken ct)
    {
        var tokens = await _db.RefreshTokens
            .Where(rt => rt.UserId == new UserId(command.UserId) && !rt.IsRevoked)
            .ToListAsync(ct);

        foreach (var token in tokens)
        {
            token.Revoke(_requestContext.IpAddress);
        }

        await _db.SaveChangesAsync(ct);
    }
}
