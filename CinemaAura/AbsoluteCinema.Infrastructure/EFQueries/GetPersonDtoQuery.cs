using AbsoluteCinema.Application.Features.Persons.Queries;
using AbsoluteCinema.Domain.Entities;
using AbsoluteCinema.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace AbsoluteCinema.Infrastructure.EFQueries;

public class GetPersonDtoQuery(CinemaDbContext db) : IGetPersonDtoQuery
{
    private readonly CinemaDbContext _db = db;

    public async Task<GetPersonResponse?> ExecuteAsync(PersonId personId, CancellationToken ct)
    {
        return await _db.Persons
            .AsNoTracking()
            .Where(p => p.Id == personId)
            .Select(p => new GetPersonResponse(
                p.Id.Id,
                p.Name,
                p.Bio,
                p.BirthDate,
                p.PersonRole,
                p.Media != null ? p.Media.Url : null,
                p.MediaId.HasValue ? p.MediaId.Value.Id : (Guid?)null
            ))
            .FirstOrDefaultAsync(ct);
    }
}