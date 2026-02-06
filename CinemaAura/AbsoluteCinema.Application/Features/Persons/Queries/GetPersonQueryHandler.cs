using AbsoluteCinema.Application.Repository;
using AbsoluteCinema.Domain.Entities;
using AbsoluteCinema.Domain.Enums;
using AbsoluteCinema.Domain.Exceptions;
using MediatR;

namespace AbsoluteCinema.Application.Features.Persons.Queries;

public record GetPersonQuery(Guid PersonId) : IRequest<GetPersonResponse>;

public record GetPersonResponse(
    Guid PersonId,
    string FullName,
    string? Bio,
    DateTime BirthDate,
    PersonRole Role,
    string? PhotoUrl,
    Guid? MediaId
);

public class GetPersonQueryHandler(IGetPersonDtoQuery personQuery)
    : IRequestHandler<GetPersonQuery, GetPersonResponse>
{
    private readonly IGetPersonDtoQuery _personQuery = personQuery;

    public async Task<GetPersonResponse> Handle(GetPersonQuery request, CancellationToken ct)
    {
        var personId = new PersonId(request.PersonId);
        
        var person = await _personQuery.ExecuteAsync(personId, ct)
            ?? throw new DomainException($"Person with ID {request.PersonId} not found.");

        return person;
    }
}