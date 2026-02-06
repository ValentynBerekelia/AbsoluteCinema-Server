using AbsoluteCinema.Application.Features.Auth.Command.LoginUser;

namespace AbsoluteCinema.Application.Features.Auth.Command.LoginUser;

public interface ILoginUserCommand
{
    Task<LoginUserResponse> ExecuteAsync(LoginUserCommand command, CancellationToken ct);
}
