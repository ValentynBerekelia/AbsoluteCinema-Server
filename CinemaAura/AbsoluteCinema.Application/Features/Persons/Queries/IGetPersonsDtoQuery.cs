namespace AbsoluteCinema.Application.Features.Persons.Queries;

public interface IGetPersonsDtoQuery
{
    Task<IEnumerable<PersonListItem>> ExecuteAsync(GetPersonsQuery query, CancellationToken ct);
}