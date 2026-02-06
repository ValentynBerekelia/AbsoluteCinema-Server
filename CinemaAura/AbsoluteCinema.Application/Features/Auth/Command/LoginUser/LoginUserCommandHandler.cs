using MediatR;

namespace AbsoluteCinema.Application.Features.Auth.Command.LoginUser;

public class LoginUserCommandHandler(ILoginUserCommand loginUser) : IRequestHandler<LoginUserCommand, LoginUserResponse>
{
    private readonly ILoginUserCommand _loginUser = loginUser;

    public async Task<LoginUserResponse> Handle(LoginUserCommand request, CancellationToken ct)
    {
        return await _loginUser.ExecuteAsync(request, ct);
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