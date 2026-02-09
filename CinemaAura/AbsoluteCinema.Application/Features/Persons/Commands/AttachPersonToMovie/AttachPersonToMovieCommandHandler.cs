using AbsoluteCinema.Application.Repository;
using AbsoluteCinema.Domain.Entities;
using AbsoluteCinema.Domain.Enums;
using AbsoluteCinema.Domain.Exceptions;
using AbsoluteCinema.Domain.Specifications;
using MediatR;

namespace AbsoluteCinema.Application.Features.Persons.Commands.AttachPersonToMovie
{
    public record AttachPersonToMovieCommand(
    Guid MovieId,
    Guid PersonId,
    PersonRole Role
    ) : IRequest<Unit>;

    public class AttachPersonToMovieCommandHandler(IMovieRepository movies,
        IPersonRepository persons,
        IUnitOfWork unitOfWork) : IRequestHandler<AttachPersonToMovieCommand, Unit>
    {
        private readonly IMovieRepository _movies = movies;
        private readonly IPersonRepository _persons = persons;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        public async Task<Unit> Handle(AttachPersonToMovieCommand command, CancellationToken ct)
        {
            var movieId = new MovieId(command.MovieId);
            var personId = new PersonId(command.PersonId);

            var movie = await _movies.GetBySpecificationAsync(new MovieWithPersonsSpec(movieId), ct)
                ?? throw new DomainException($"Movie with ID {command.MovieId} not found.");

            var person = await _persons.GetByIdForUpdateAsync(personId, ct)
                ?? throw new DomainException($"Person with ID {command.PersonId} not found.");

            if (person.PersonRole != command.Role)
            {
                person.ChangePersonRole(command.Role);
            }

            movie.AddPerson(person);
            _movies.Update(movie);
            await _unitOfWork.SaveChangesAsync(ct);
            return Unit.Value;
        }
    }
}
