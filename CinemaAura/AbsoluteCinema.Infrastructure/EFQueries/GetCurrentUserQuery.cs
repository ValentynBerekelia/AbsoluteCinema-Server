using AbsoluteCinema.Application.Features.Auth.Queries.GetCurrentUser;
using AbsoluteCinema.Domain.Entities;
using AbsoluteCinema.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace AbsoluteCinema.Infrastructure.EFQueries;

public class GetCurrentUserQueryInfra(CinemaDbContext db) : IGetCurrentUserQuery
{
    private readonly CinemaDbContext _db = db;

    public async Task<GetCurrentUserResponse?> ExecuteAsync(Guid userId, CancellationToken ct)
    {
        var user = await _db.Users
            .Include(u => u.Roles)
                .ThenInclude(r => r.Permissions)
            .FirstOrDefaultAsync(u => u.Id == new UserId(userId), ct);

        if (user == null)
        {
            return null;
        }

        var roles = user.Roles.Select(r => r.Name).ToList();
        var permissions = user.Roles
            .SelectMany(r => r.Permissions)
            .Select(p => p.Code.Value)
            .Distinct()
            .ToList();

        return new GetCurrentUserResponse(
            user.Id.Id,
            user.UserName,
            roles,
            permissions
        );
    }
}
