using AbsoluteCinema.Application.Abstractions;
using AbsoluteCinema.Application.Common.Exceptions;
using AbsoluteCinema.Application.Repository;
using AbsoluteCinema.Domain.Entities;
using AbsoluteCinema.Domain.Exceptions;
using AbsoluteCinema.Domain.Specifications;
using MediatR;

namespace AbsoluteCinema.Application.Features.Auth.Command.LoginUser;

public class LoginUserCommandHandler(IUserRepository users, IPasswordHasher hasher, IRequestContext context, ITokenProvider tokenProvider) : IRequestHandler<LoginUserCommand, LoginUserResponse>
{
    private readonly IUserRepository _users = users;
    private readonly IPasswordHasher _hasher = hasher;
    private readonly IRequestContext _context = context;
    private readonly ITokenProvider _tokenProvider = tokenProvider;

    public async Task<LoginUserResponse> Handle(LoginUserCommand command, CancellationToken ct)
    {
        var user = await _users.GetBySpecificationAsync(new UserByEmailSpec(command.Email), ct);
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
        var (refreshToken, _) = _tokenProvider.GenerateRefreshToken();

        return new LoginUserResponse(
            user.Id.Id,
            user.UserName,
            user.Email,
            refreshToken,
            accessToken
        );
    }
}

public record LoginUserCommand : IRequest<LoginUserResponse>
{
    public required string Email { get; init; }
    public required string Password { get; init; }
}

public record LoginUserResponse
(
    Guid UserId,
    string Token,
    string UserName,
    string RefreshToken,
    string AccessToken
);