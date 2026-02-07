using AbsoluteCinema.Application.Repository;
using AbsoluteCinema.Domain.Entities;
using AbsoluteCinema.Domain.Enums;
using MediatR;

namespace AbsoluteCinema.Application.Features.Persons.Commands.CreatePerson;

public record CreatePersonCommand(
    string FullName,
    string? Bio,
    DateTime BirthDate,
    PersonRole Role
) : IRequest<CreatePersonResponse>;

public record CreatePersonResponse(
    Guid PersonId,
    string FullName,
    string? Bio,
    DateTime BirthDate,
    PersonRole Role
);

public class CreatePersonCommandHandler(
    IPersonRepository persons,
    IUnitOfWork unitOfWork) : IRequestHandler<CreatePersonCommand, CreatePersonResponse>
{
    private readonly IPersonRepository _persons = persons;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<CreatePersonResponse> Handle(CreatePersonCommand command, CancellationToken ct)
    {
        var person = Person.Create(
            name: command.FullName,
            bio: command.Bio ?? string.Empty,
            birthDate: command.BirthDate,
            personRole: command.Role
        );

        await _persons.AddAsync(person, ct);
        await _unitOfWork.SaveChangesAsync(ct);

        return new CreatePersonResponse(
            person.Id.Id,
            person.Name,
            person.Bio,
            person.BirthDate,
            person.PersonRole
        );
    }
}