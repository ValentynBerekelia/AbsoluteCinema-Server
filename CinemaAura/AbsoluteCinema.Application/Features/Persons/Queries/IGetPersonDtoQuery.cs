using AbsoluteCinema.Domain.Entities;

namespace AbsoluteCinema.Application.Features.Persons.Queries;

public interface IGetPersonDtoQuery
{
    Task<GetPersonResponse?> ExecuteAsync(PersonId personId, CancellationToken ct);
}