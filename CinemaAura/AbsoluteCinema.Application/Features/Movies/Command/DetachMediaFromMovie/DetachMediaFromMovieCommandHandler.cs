using AbsoluteCinema.Application.Repository;
using AbsoluteCinema.Domain.Entities;
using AbsoluteCinema.Domain.Exceptions;
using AbsoluteCinema.Domain.Specifications;
using MediatR;

namespace AbsoluteCinema.Application.Features.Movies.Command.DetachMediaFromMovie;

public record DetachMediaFromMovieCommand(Guid MovieId, Guid MediaId) : IRequest<Unit>;

public class DetachMediaFromMovieCommandHandler(
    IMovieRepository movies,
    IUnitOfWork unitOfWork) : IRequestHandler<DetachMediaFromMovieCommand, Unit>
{
    private readonly IMovieRepository _movies = movies;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Unit> Handle(DetachMediaFromMovieCommand command, CancellationToken ct)
    {
        var movieId = new MovieId(command.MovieId);
        var mediaId = new MediaId(command.MediaId);

        var movie = await _movies.GetBySpecificationAsync(new MovieWithMediasSpec(movieId), ct)
            ?? throw new DomainException($"Movie with ID {command.MovieId} not found.");

        if (!movie.Medias.Any(m => m.Id == mediaId))
            throw new DomainException($"Media with ID {command.MediaId} is not attached to movie {command.MovieId}.");

        movie.RemoveMedia(mediaId);

        _movies.Update(movie);
        await _unitOfWork.SaveChangesAsync(ct);

        return Unit.Value;
    }
}
