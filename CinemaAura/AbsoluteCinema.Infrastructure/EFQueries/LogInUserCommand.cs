using AbsoluteCinema.Application.Abstractions;
using AbsoluteCinema.Application.Common.Exceptions;
using AbsoluteCinema.Application.Features.Auth.Command.LoginUser;
using AbsoluteCinema.Domain.Entities;
using AbsoluteCinema.Domain.Exceptions;
using AbsoluteCinema.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace AbsoluteCinema.Infrastructure.EFQueries;

public class LogInUserCommand(CinemaDbContext db, ITokenProvider tokenProvider, IPasswordHasher hasher, IRequestContext requestContext) : ILoginUserCommand
{
    private readonly CinemaDbContext _db = db;
    private readonly ITokenProvider _tokenProvider = tokenProvider;
    private readonly IPasswordHasher _hasher = hasher;
    private readonly IRequestContext _requestContext = requestContext;

    public async Task<LoginUserResponse> ExecuteAsync(LoginUserCommand command, CancellationToken ct)
    {
        var user = await _db.Users
            .FirstOrDefaultAsync(u => u.Email == command.Email, ct);
        
        if (user == null)
        {
            throw new UserNotFoundException(command.Email);
        }

        var isValidPassword = _hasher.Verify(command.Password, user.PasswordHash);
        if (!isValidPassword)
        {
            throw new DomainException("Invalid credentials.");
        }

        var accessToken = _tokenProvider.GenerateAccessToken(user);
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
            user.Id.Id,
            user.UserName,
            user.Email,
            refreshToken,
            accessToken
        );
    }
}
