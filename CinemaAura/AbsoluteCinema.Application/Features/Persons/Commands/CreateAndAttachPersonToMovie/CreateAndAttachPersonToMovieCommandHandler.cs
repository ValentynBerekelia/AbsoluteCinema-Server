using AbsoluteCinema.Application.Repository;
using AbsoluteCinema.Domain.Entities;
using AbsoluteCinema.Domain.Enums;
using AbsoluteCinema.Domain.Exceptions;
using AbsoluteCinema.Domain.Specifications;
using MediatR;

namespace AbsoluteCinema.Application.Features.Persons.Commands.CreateAndAttachPersonToMovie
{
    public record CreateAndAttachPersonToMovieCommand(
        Guid MovieId,
        string FullName,
        string? Bio,
        DateTime BirthDate,
        PersonRole Role
    ) : IRequest<CreateAndAttachPersonToMovieResponse>;

    public record CreateAndAttachPersonToMovieResponse(
        Guid PersonId,
        string FullName,
        string? Bio,
        DateTime BirthDate,
        PersonRole Role
    );
    public class CreateAndAttachPersonToMovieCommandHandler(IMovieRepository movies,
        IPersonRepository persons,
        IUnitOfWork unitOfWork) : IRequestHandler<CreateAndAttachPersonToMovieCommand, CreateAndAttachPersonToMovieResponse>
    {
        private readonly IMovieRepository _movies = movies;
        private readonly IPersonRepository _persons = persons;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        public async Task<CreateAndAttachPersonToMovieResponse> Handle(CreateAndAttachPersonToMovieCommand command, CancellationToken ct)
        {
            var movieId = new MovieId(command.MovieId);
            var movie = await _movies.GetBySpecificationAsync(new MovieWithPersonsSpec(movieId), ct)
                ?? throw new DomainException($"Movie with ID {command.MovieId} not found.");

            var person = Person.Create(
                name: command.FullName,
                bio: command.Bio ?? string.Empty,
                birthDate: command.BirthDate,
                personRole: command.Role
            );

            await _persons.AddAsync(person, ct);

            movie.AddPerson(person);

            _movies.Update(movie);
            await _unitOfWork.SaveChangesAsync(ct);

            return new CreateAndAttachPersonToMovieResponse(
                person.Id.Id,
                person.Name,
                person.Bio,
                person.BirthDate,
                person.PersonRole
            );
        }
    }
}
