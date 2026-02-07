using AbsoluteCinema.Application.Repository;
using AbsoluteCinema.Domain.Entities;
using AbsoluteCinema.Domain.Exceptions;
using MediatR;

namespace AbsoluteCinema.Application.Features.Persons.Commands.DeletePerson;

public record DeletePersonCommand(Guid PersonId) : IRequest<Unit>;

public class DeletePersonCommandHandler(
    IPersonRepository persons,
    IUnitOfWork unitOfWork) : IRequestHandler<DeletePersonCommand, Unit>
{
    private readonly IPersonRepository _persons = persons;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Unit> Handle(DeletePersonCommand command, CancellationToken ct)
    {
        var personId = new PersonId(command.PersonId);

        var exists = await _persons.AnyAsync(personId, ct);
        if (!exists)
            throw new DomainException($"Person with ID {command.PersonId} not found.");

        await _persons.DeleteAsync(personId, ct);
        await _unitOfWork.SaveChangesAsync(ct);

        return Unit.Value;
    }
}