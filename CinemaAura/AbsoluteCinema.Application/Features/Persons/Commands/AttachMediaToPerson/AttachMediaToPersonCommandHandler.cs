using AbsoluteCinema.Application.Repository;
using AbsoluteCinema.Domain.Entities;
using AbsoluteCinema.Domain.Enums;
using AbsoluteCinema.Domain.Exceptions;
using MediatR;

namespace AbsoluteCinema.Application.Features.Persons.Commands.AttachMediaToPerson;

public record AttachMediaToPersonCommand(Guid PersonId, string Url) : IRequest<AttachMediaToPersonResponse>;

public record AttachMediaToPersonResponse(Guid PersonId, Guid MediaId, string Url);

public class AttachMediaToPersonCommandHandler(
    IPersonRepository persons,
    IMediaRepository medias,
    IUnitOfWork unitOfWork) : IRequestHandler<AttachMediaToPersonCommand, AttachMediaToPersonResponse>
{
    private readonly IPersonRepository _persons = persons;
    private readonly IMediaRepository _medias = medias;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<AttachMediaToPersonResponse> Handle(AttachMediaToPersonCommand command, CancellationToken ct)
    {
        var personId = new PersonId(command.PersonId);

        var person = await _persons.GetByIdForUpdateAsync(personId, ct)
            ?? throw new DomainException($"Person with ID {command.PersonId} not found.");

        // create Media with PersonImage type
        var media = Media.Create(MediaType.PersonImage, command.Url);

        await _medias.AddAsync(media, ct);

        // domain method validates MediaType
        person.ChangeMedia(media);

        _persons.Update(person);
        await _unitOfWork.SaveChangesAsync(ct);

        return new AttachMediaToPersonResponse(command.PersonId, media.Id.Id, media.Url);
    }
}