using AbsoluteCinema.Application.DTOs.User;
using MediatR;

namespace AbsoluteCinema.Application.Features.Users.Queries;

public class GetUsersWithTicketsCountQueryHandler(IGetUsersWithTicketsCountQueryDto getUsersWithTicketsCountQueryDto)
    : IRequestHandler<GetUsersWithTicketsCountQuery, GetUsersWithTicketsCountResponse>
{
    private readonly IGetUsersWithTicketsCountQueryDto _getUsersWithTicketsCountQueryDto = getUsersWithTicketsCountQueryDto;

    public async Task<GetUsersWithTicketsCountResponse> Handle(GetUsersWithTicketsCountQuery request, CancellationToken ct)
    {
        var users = await _getUsersWithTicketsCountQueryDto.ExecuteAsync(request, ct); 
        return users;
    }
}

public record GetUsersWithTicketsCountQuery (
    int PageNumber = 1,
    int PageSize = 10,
    string? SearchTerm = null
): IRequest<GetUsersWithTicketsCountResponse>;

public record GetUsersWithTicketsCountResponse (
    List<UserWithTicketsCountDto> Users,
    int TotalCount
);