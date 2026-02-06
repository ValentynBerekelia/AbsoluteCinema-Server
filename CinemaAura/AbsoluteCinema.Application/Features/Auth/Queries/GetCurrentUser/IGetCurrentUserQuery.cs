namespace AbsoluteCinema.Application.Features.Auth.Queries.GetCurrentUser;

public interface IGetCurrentUserQuery
{
    Task<GetCurrentUserResponse?> ExecuteAsync(Guid userId, CancellationToken ct);
}
