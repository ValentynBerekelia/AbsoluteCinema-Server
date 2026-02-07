using AbsoluteCinema.Application.Abstractions;
using AbsoluteCinema.Application.Features.Auth;
using AbsoluteCinema.Domain.Entities;
using AbsoluteCinema.Domain.Exceptions;
using AbsoluteCinema.Domain.ValueObjects;
using AbsoluteCinema.Infrastructure.Persistence;
using AbsoluteCinema.Infrastructure.Security;
using Microsoft.EntityFrameworkCore;

namespace AbsoluteCinema.Infrastructure.EFQueries;

public class RegisterUserCommand(CinemaDbContext db, ITokenProvider tokenProvider, IPasswordHasher hasher, IRequestContext requestContext) : ICreateUserCommand
{
    private readonly CinemaDbContext _db = db;
    private readonly ITokenProvider _tokenProvider = tokenProvider;
    private readonly IPasswordHasher _hasher = hasher;
    private readonly IRequestContext _requestContext = requestContext;

    public async Task<CreateUserResponse> ExecuteAsync(CreateUserCommand request, CancellationToken ct)
    {
        if (await _db.Users.AnyAsync(u => u.Email == request.Email, ct))
        {
            throw new UserAlreadyExistsException(request.Email);
        }

        var passwordHash = _hasher.Hash(request.Password);
        var user = User.Create(request.UserName, passwordHash, request.Email);

        db.Users.Add(user);

        var accessToken = _tokenProvider.GenerateAccessToken(user);
        var (refreshToken, refreshTokenHash) = _tokenProvider.GenerateRefreshToken();

        var now = DateTime.UtcNow;

        var rt = RefreshToken.Create(
            user.Id,
            refreshTokenHash,
            expiresAt: now.AddDays(14),
            createdByIp: _requestContext.IpAddress
        );

        db.RefreshTokens.Add(rt);

        await _db.SaveChangesAsync(ct);

        return new CreateUserResponse(
            user.Id.Id,
            accessToken,
            refreshToken
    );

}
}