using AbsoluteCinema.Application.Repository;
using AbsoluteCinema.Domain.Entities;
using AbsoluteCinema.Domain.Exceptions;
using AbsoluteCinema.Domain.Specifications;
using MediatR;

namespace AbsoluteCinema.Application.Features.Persons.Commands.DetachPersonFromMovie;

public record DetachPersonFromMovieCommand(Guid MovieId, Guid PersonId) : IRequest<Unit>;

public class DetachPersonFromMovieCommandHandler(
    IMovieRepository movies,
    IUnitOfWork unitOfWork) : IRequestHandler<DetachPersonFromMovieCommand, Unit>
{
    private readonly IMovieRepository _movies = movies;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Unit> Handle(DetachPersonFromMovieCommand command, CancellationToken ct)
    {
        var movieId = new MovieId(command.MovieId);
        var personId = new PersonId(command.PersonId);

        var movie = await _movies.GetBySpecificationAsync(new MovieWithPersonsSpec(movieId), ct)
            ?? throw new DomainException($"Movie with ID {command.MovieId} not found.");

        if (!movie.Persons.Any(p => p.Id == personId))
            throw new DomainException($"Person with ID {command.PersonId} is not attached to movie {command.MovieId}.");

        movie.RemovePersonById(personId);

        _movies.Update(movie);
        await _unitOfWork.SaveChangesAsync(ct);

        return Unit.Value;
    }
}
