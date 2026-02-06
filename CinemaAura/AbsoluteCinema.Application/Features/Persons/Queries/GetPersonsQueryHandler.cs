using AbsoluteCinema.Domain.Enums;
using MediatR;

namespace AbsoluteCinema.Application.Features.Persons.Queries;

public record GetPersonsQuery(
    string? SearchTerm,
    PersonRole? Role,
    int Limit
) : IRequest<IEnumerable<PersonListItem>>;

public record PersonListItem(
    Guid PersonId,
    string FullName,
    PersonRole Role,
    string? PhotoUrl
);

public class GetPersonsQueryHandler(IGetPersonsDtoQuery personsQuery)
    : IRequestHandler<GetPersonsQuery, IEnumerable<PersonListItem>>
{
    private readonly IGetPersonsDtoQuery _personsQuery = personsQuery;

    public async Task<IEnumerable<PersonListItem>> Handle(GetPersonsQuery request, CancellationToken ct)
    {
        return await _personsQuery.ExecuteAsync(request, ct);
    }
}