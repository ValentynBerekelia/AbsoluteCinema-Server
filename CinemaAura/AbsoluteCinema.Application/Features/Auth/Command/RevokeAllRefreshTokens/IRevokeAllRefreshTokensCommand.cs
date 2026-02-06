namespace AbsoluteCinema.Application.Features.Auth.Command.RevokeAllRefreshTokens;

public interface IRevokeAllRefreshTokensCommand
{
    Task ExecuteAsync(RevokeAllRefreshTokensCommand command, CancellationToken ct);
}
