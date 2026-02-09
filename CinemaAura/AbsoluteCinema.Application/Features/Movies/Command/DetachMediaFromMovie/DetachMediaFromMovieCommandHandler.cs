using AbsoluteCinema.Application.Repository;
using AbsoluteCinema.Domain.Entities;
using AbsoluteCinema.Domain.Exceptions;
using AbsoluteCinema.Domain.Specifications;
using MediatR;

namespace AbsoluteCinema.Application.Features.Movies.Command.DetachMediaFromMovie;

public record DetachMediaFromMovieCommand(Guid MovieId, Guid MediaId) : IRequest<Unit>;

public class DetachMediaFromMovieCommandHandler(
    IMovieRepository movies,
    IMediaRepository medias,
    IStorageService storageService,
    IUnitOfWork unitOfWork) : IRequestHandler<DetachMediaFromMovieCommand, Unit>
{
    public async Task<Unit> Handle(DetachMediaFromMovieCommand command, CancellationToken ct)
    {
        var movieId = new MovieId(command.MovieId);
        var mediaId = new MediaId(command.MediaId);

        var movie = await movies.GetBySpecificationAsync(new MovieWithMediasSpec(movieId), ct)
            ?? throw new DomainException($"Movie with ID {command.MovieId} not found.");

        var media = movie.Medias.FirstOrDefault(m => m.Id == mediaId)
            ?? throw new DomainException($"Media with ID {command.MediaId} not found in this movie.");

        if (IsSupabaseInternalFile(media.Url))
        {
            var relativePath = ExtractRelativePath(media.Url);
            await storageService.DeleteFileAsync(relativePath);
        }

        movie.RemoveMedia(mediaId);
        await medias.DeleteAsync(media.Id, ct);
        await unitOfWork.SaveChangesAsync(ct);
        return Unit.Value;
    }

    private bool IsSupabaseInternalFile(string url) => url.Contains("supabase.co");
    
    private string ExtractRelativePath(string url) 
    {
        return url.Split("/public/Images/").Last();
    }
}