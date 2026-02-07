using AbsoluteCinema.Application.Features.Auth.Command.RefreshToken;

namespace AbsoluteCinema.Application.Features.Auth.Command.RefreshToken;

public interface IRefreshTokenCommand
{
    Task<RefreshTokenResponse> ExecuteAsync(RefreshTokenCommand command, CancellationToken ct);
}
