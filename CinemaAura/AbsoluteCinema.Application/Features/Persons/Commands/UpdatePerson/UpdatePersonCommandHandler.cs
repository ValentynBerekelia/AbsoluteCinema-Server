using AbsoluteCinema.Application.Repository;
using AbsoluteCinema.Domain.Entities;
using AbsoluteCinema.Domain.Enums;
using AbsoluteCinema.Domain.Exceptions;
using MediatR;

namespace AbsoluteCinema.Application.Features.Persons.Commands.UpdatePerson;

public record UpdatePersonCommand(
    Guid PersonId,
    string FullName,
    string? Bio,
    DateTime BirthDate,
    PersonRole Role
) : IRequest<Unit>;

public class UpdatePersonCommandHandler(
    IPersonRepository persons,
    IUnitOfWork unitOfWork) : IRequestHandler<UpdatePersonCommand, Unit>
{
    private readonly IPersonRepository _persons = persons;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Unit> Handle(UpdatePersonCommand command, CancellationToken ct)
    {
        var personId = new PersonId(command.PersonId);

        var person = await _persons.GetByIdForUpdateAsync(personId, ct)
            ?? throw new DomainException($"Person with ID {command.PersonId} not found.");

        person.ChangeName(command.FullName);
        person.ChangeBio(command.Bio ?? string.Empty);
        person.ChangeBirthDate(command.BirthDate);
        person.ChangePersonRole(command.Role);

        _persons.Update(person);
        await _unitOfWork.SaveChangesAsync(ct);

        return Unit.Value;
    }
}