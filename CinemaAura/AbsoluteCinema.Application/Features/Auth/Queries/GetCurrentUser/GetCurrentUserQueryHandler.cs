using MediatR;

namespace AbsoluteCinema.Application.Features.Auth.Queries.GetCurrentUser;

public class GetCurrentUserQueryHandler(IGetCurrentUserQuery getCurrentUser) : IRequestHandler<GetCurrentUserQuery, GetCurrentUserResponse?>
{
    private readonly IGetCurrentUserQuery _getCurrentUser = getCurrentUser;

    public async Task<GetCurrentUserResponse?> Handle(GetCurrentUserQuery request, CancellationToken ct)
    {
        return await _getCurrentUser.ExecuteAsync(request.UserId, ct);
    }
}

public record GetCurrentUserQuery(Guid UserId) : IRequest<GetCurrentUserResponse?>;

public record GetCurrentUserResponse(
    Guid UserId,
    IReadOnlyList<string> Roles,
    IReadOnlyList<string> Permissions
);
