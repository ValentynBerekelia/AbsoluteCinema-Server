using System.Security.Cryptography;
using System.Text;
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


        var passHash = Encoding.UTF8.GetBytes(_hasher.Hash(request.Password));
        var salt = Encoding.UTF8.GetBytes(_hasher.Hash(RandomNumberGenerator.GetBytes(16).ToString()!));
        var user = User.Create(request.UserName, PasswordHash.Create(passHash, salt), request.Email);

        db.Users.Add(user);

        var accessToken = _tokenProvider.GenerateAccessToken(user);
        var refreshToken = _tokenProvider.GenerateRefreshToken();

        var now = DateTime.UtcNow;

        var rt = RefreshToken.Create(
            user.Id,
            refreshToken,
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