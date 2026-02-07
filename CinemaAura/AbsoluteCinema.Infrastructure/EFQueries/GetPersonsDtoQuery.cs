using AbsoluteCinema.Application.Features.Persons.Queries;
using AbsoluteCinema.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace AbsoluteCinema.Infrastructure.EFQueries;

public class GetPersonsDtoQuery(CinemaDbContext db) : IGetPersonsDtoQuery
{
    private readonly CinemaDbContext _db = db;

    public async Task<IEnumerable<PersonListItem>> ExecuteAsync(GetPersonsQuery query, CancellationToken ct)
    {
        var personsQuery = _db.Persons.AsNoTracking();

        // filter by search (for autocomplete on frontend)
        if (!string.IsNullOrWhiteSpace(query.SearchTerm))
        {
            var searchLower = query.SearchTerm.ToLower();
            personsQuery = personsQuery.Where(p =>
                p.Name.ToLower().Contains(searchLower));
        }

        // filter by role
        if (query.Role.HasValue)
        {
            personsQuery = personsQuery.Where(p => p.PersonRole == query.Role.Value);
        }

        return await personsQuery
            .OrderBy(p => p.Name)
            .Take(query.Limit)
            .Select(p => new PersonListItem(
                p.Id.Id,
                p.Name,
                p.PersonRole,
                p.Media != null ? p.Media.Url : null
            ))
            .ToListAsync(ct);
    }
}