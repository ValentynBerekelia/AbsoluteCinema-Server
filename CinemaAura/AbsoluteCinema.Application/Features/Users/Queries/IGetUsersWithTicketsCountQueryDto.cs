namespace AbsoluteCinema.Application.Features.Users.Queries;
public interface IGetUsersWithTicketsCountQueryDto
{
    Task<GetUsersWithTicketsCountResponse> ExecuteAsync(GetUsersWithTicketsCountQuery query, CancellationToken ct);
}