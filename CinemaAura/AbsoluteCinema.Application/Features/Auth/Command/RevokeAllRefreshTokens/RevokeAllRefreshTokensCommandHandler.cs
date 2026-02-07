using MediatR;

namespace AbsoluteCinema.Application.Features.Auth.Command.RevokeAllRefreshTokens;

public class RevokeAllRefreshTokensCommandHandler(IRevokeAllRefreshTokensCommand revokeAll) : IRequestHandler<RevokeAllRefreshTokensCommand>
{
    private readonly IRevokeAllRefreshTokensCommand _revokeAll = revokeAll;

    public async Task Handle(RevokeAllRefreshTokensCommand request, CancellationToken ct)
    {
        await _revokeAll.ExecuteAsync(request, ct);
    }
}

public record RevokeAllRefreshTokensCommand : IRequest
{
    public required Guid UserId { get; init; }
}
