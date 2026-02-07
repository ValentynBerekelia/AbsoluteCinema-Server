using System.Security.Claims;
using AbsoluteCinema.Application.Abstractions;
using AbsoluteCinema.Application.Features.Auth;
using AbsoluteCinema.Domain.Entities;
using AbsoluteCinema.Domain.Exceptions;
using AbsoluteCinema.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace AbsoluteCinema.Infrastructure.EFQueries;

public class RegisterUserCommand(
    CinemaDbContext db,
    ITokenProvider tokenProvider,
    IPasswordHasher hasher,
    IRequestContext requestContext
) : ICreateUserCommand
{
    private readonly CinemaDbContext _db = db;
    private readonly ITokenProvider _tokenProvider = tokenProvider;
    private readonly IPasswordHasher _hasher = hasher;
    private readonly IRequestContext _requestContext = requestContext;

    public async Task<CreateUserResponse> ExecuteAsync(CreateUserCommand request, CancellationToken ct)
    {
        if (await _db.Users.AnyAsync(u => u.Email == request.Email, ct))
            throw new UserAlreadyExistsException(request.Email);

        var passwordHash = _hasher.Hash(request.Password);
        var user = User.Create(request.UserName, passwordHash, request.Email);

        var defaultRole = await _db.Roles
            .Include(r => r.Permissions)
            .FirstOrDefaultAsync(r => r.Name == "User", ct);

        if (defaultRole is null)
            throw new DomainException("Default role 'User' not found. Seed roles first.");

        user.AddRole(defaultRole);

        _db.Users.Add(user);

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

        return new CreateUserResponse(
            user.Id.Id,
            accessToken,
            refreshToken
        );
    }
}
