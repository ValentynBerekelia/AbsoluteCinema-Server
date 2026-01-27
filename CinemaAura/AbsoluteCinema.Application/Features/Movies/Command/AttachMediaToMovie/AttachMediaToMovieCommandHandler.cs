using AbsoluteCinema.Application.Repository;
using AbsoluteCinema.Domain.Entities;
using AbsoluteCinema.Domain.Exceptions;
using AbsoluteCinema.Domain.Specifications;
using MediatR;

namespace AbsoluteCinema.Application.Features.Movies.Command.AttachMediaToMovie;

public class AttachMediaToMovieCommandHandler(
    IMovieRepository movies,
    IMediaRepository medias,
    IUnitOfWork unitOfWork) : IRequestHandler<AttachMediaToMovieCommand, Unit>
{
    private readonly IMovieRepository _movies = movies;
    private readonly IMediaRepository _medias = medias;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Unit> Handle(AttachMediaToMovieCommand command, CancellationToken ct)
    {
        // EF tracking obj
        var movie = await _movies.GetBySpecificationAsync(new MovieWithMediasSpec(
            new MovieId(command.MovieId)), ct)
            ?? throw new DomainException($"Movie with ID {command.MovieId} not found.");

        var media = await _medias.GetByIdAsync(new MediaId(command.MediaId), ct)
            ?? throw new DomainException($"Media with ID {command.MediaId} not found.");

        // Domain logic
        movie.AddMedia(media);

        _movies.Update(movie);
        await _unitOfWork.SaveChangesAsync(ct);

        return Unit.Value;
    }
}

public record AttachMediaToMovieCommand(
    Guid MovieId,
    Guid MediaId
) : IRequest<Unit>;