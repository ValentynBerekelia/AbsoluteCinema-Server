using AbsoluteCinema.Application.Abstractions;
using AbsoluteCinema.Application.Common.Exceptions;
using AbsoluteCinema.Application.Features.Auth.Command.LoginUser;
using AbsoluteCinema.Domain.Entities;
using AbsoluteCinema.Domain.Exceptions;
using AbsoluteCinema.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace AbsoluteCinema.Infrastructure.EFQueries;

public class LogInUserCommand(
    CinemaDbContext db,
    ITokenProvider tokenProvider,
    IPasswordHasher hasher,
    IRequestContext requestContext) : ILoginUserCommand
{
    private readonly CinemaDbContext _db = db;
    private readonly ITokenProvider _tokenProvider = tokenProvider;
    private readonly IPasswordHasher _hasher = hasher;
    private readonly IRequestContext _requestContext = requestContext;

    public async Task<LoginUserResponse> ExecuteAsync(LoginUserCommand command, CancellationToken ct)
    {
        var user = await _db.Users
            .Include(u => u.Roles)
                .ThenInclude(r => r.Permissions)
            .FirstOrDefaultAsync(u => u.Email == command.Email, ct);

        if (user is null)
            throw new UserNotFoundException(command.Email);

        var isValidPassword = _hasher.Verify(command.Password, user.PasswordHash);
        if (!isValidPassword)
            throw new DomainException("Invalid credentials.");

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

        var (refreshToken, refreshTokenHash) = _tokenProvider.GenerateRefreshToken();

        var now = DateTime.UtcNow;
        var rt = RefreshToken.Create(
            user.Id,
            refreshTokenHash,
            expiresAt: now.AddDays(14),
            createdByIp: _requestContext.IpAddress
        );

        _db.RefreshTokens.Add(rt);
        await _db.SaveChangesAsync(ct);

        return new LoginUserResponse(
            UserId: user.Id.Id,
            Email: user.Email,
            UserName: user.UserName,
            RefreshToken: refreshToken,
            AccessToken: accessToken
        );
    }
}
