namespace AbsoluteCinema.Application.Features.Auth.Command.Logout;

public interface ILogoutCommand
{
    Task ExecuteAsync(LogoutCommand command, CancellationToken ct);
}
