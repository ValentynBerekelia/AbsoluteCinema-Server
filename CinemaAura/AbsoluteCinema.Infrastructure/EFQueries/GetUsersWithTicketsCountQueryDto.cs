using AbsoluteCinema.Application.DTOs.User;
using AbsoluteCinema.Application.Features.Users.Queries;
using AbsoluteCinema.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace AbsoluteCinema.Infrastructure.EFQueries;



public class GetUsersWithTicketsCountQueryDto(CinemaDbContext db) : IGetUsersWithTicketsCountQueryDto
{
    public readonly CinemaDbContext _db = db;

    public async Task<GetUsersWithTicketsCountResponse> ExecuteAsync(GetUsersWithTicketsCountQuery query, CancellationToken ct)
    {
        var baseQuery = _db.Users.AsQueryable();

        if (!string.IsNullOrWhiteSpace(query.SearchTerm))
        {
            var term = query.SearchTerm.ToLower();
            baseQuery = baseQuery.Where(u => u.UserName.ToLower().Contains(term)
                || u.Email.ToLower().Contains(term));
        }

        var totalCount = await baseQuery.CountAsync();

        var users = await baseQuery
            .OrderBy(u => u.UserName)
            .Skip((query.PageNumber - 1) * query.PageSize)
            .Take(query.PageSize)
            .Select(u => new UserWithTicketsCountDto(
                u.Id.Id,
                u.UserName,
                u.Email,
                _db.Tickets
                    .Count(t => t.UserId == u.Id)
            ))
            .ToListAsync();

        return new GetUsersWithTicketsCountResponse(users, totalCount);
    }
}