using MediatR;

namespace AbsoluteCinema.Application.Features.Auth.Command.Logout;

public class LogoutCommandHandler(ILogoutCommand logout) : IRequestHandler<LogoutCommand>
{
    private readonly ILogoutCommand _logout = logout;

    public async Task Handle(LogoutCommand request, CancellationToken ct)
    {
        await _logout.ExecuteAsync(request, ct);
    }
}

public record LogoutCommand : IRequest
{
    public required string RefreshToken { get; init; }
}
