using AbsoluteCinema.Application.Repository;
using AbsoluteCinema.Domain.Entities;
using AbsoluteCinema.Domain.Enums;
using AbsoluteCinema.Domain.Exceptions;
using AbsoluteCinema.Domain.Specifications;
using MediatR;

namespace AbsoluteCinema.Application.Features.Movies.Command.CreateMovieAndAttachMedia
{
    public record CreateAndAttachMediaCommand(
    Guid MovieId,
    string Url,
    MediaType Type
) : IRequest<CreateAndAttachMediaResponse>;

    public record CreateAndAttachMediaResponse(Guid MovieId, Guid MediaId);

    public class CreateAndAttachMediaCommandHandler(
    IMovieRepository movies,
    IMediaRepository medias,
    IUnitOfWork unitOfWork) : IRequestHandler<CreateAndAttachMediaCommand, CreateAndAttachMediaResponse>
    {
        private readonly IMovieRepository _movies = movies;
        private readonly IMediaRepository _medias = medias;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<CreateAndAttachMediaResponse> Handle(
            CreateAndAttachMediaCommand command,
            CancellationToken ct)
        {
            var movieId = new MovieId(command.MovieId);

            // get movie with tracking and Medias collection loaded
            var movie = await _movies.GetBySpecificationAsync(new MovieWithMediasSpec(movieId), ct)
                ?? throw new DomainException($"Movie with ID {command.MovieId} not found.");

            var media = Media.Create(command.Type, command.Url);

            // save media to DB
            await _medias.AddAsync(media, ct);

            // attach Media to Movie (domain logic)
            movie.AddMedia(media);

            _movies.Update(movie);

            await _unitOfWork.SaveChangesAsync(ct);

            return new CreateAndAttachMediaResponse(command.MovieId, media.Id.Id);
        }
    }
}
