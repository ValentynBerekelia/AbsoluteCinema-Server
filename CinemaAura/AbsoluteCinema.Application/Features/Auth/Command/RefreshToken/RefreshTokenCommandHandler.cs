using MediatR;

namespace AbsoluteCinema.Application.Features.Auth.Command.RefreshToken;

public class RefreshTokenCommandHandler(IRefreshTokenCommand refreshToken) : IRequestHandler<RefreshTokenCommand, RefreshTokenResponse>
{
    private readonly IRefreshTokenCommand _refreshToken = refreshToken;

    public async Task<RefreshTokenResponse> Handle(RefreshTokenCommand request, CancellationToken ct)
    {
        return await _refreshToken.ExecuteAsync(request, ct);
    }
}

public record RefreshTokenCommand : IRequest<RefreshTokenResponse>
{
    public required string RefreshToken { get; init; }
}

public record RefreshTokenResponse(
    Guid UserId,
    string AccessToken,
    string RefreshToken
);
