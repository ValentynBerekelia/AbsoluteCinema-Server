using AbsoluteCinema.Application.Common.Exceptions;
using AbsoluteCinema.Application.Repository;
using AbsoluteCinema.Domain.Entities;
using AbsoluteCinema.Domain.Exceptions;
using AbsoluteCinema.Domain.Specifications;
using MediatR;

namespace AbsoluteCinema.Application.Features.Auth.Command.LoginUser;

public class LoginUserCommandHandler(IUserRepository users) : IRequestHandler<LoginUserCommand, LoginUserResponse>
{
    private readonly IUserRepository _users = users;

    public async Task<LoginUserResponse> Handle(LoginUserCommand command, CancellationToken ct)
    {
        var user = await _users.GetBySpecificationAsync(new UserByEmailSpec(command.Email), ct);
        if (user == null)
        {
            throw new UserNotFoundException(command.Email);
        }
        
        user.RefreshToken = command.RefreshToken;
        
    }
}

public record LoginUserCommand : IRequest<LoginUserResponse>
{
    public string Email { get; init; }
    public string Password { get; init; }
}

public record LoginUserResponse
(
    Guid UserId,
    string Token,
    string UserName,
    string RefreshToken,
    string AccessToken
);