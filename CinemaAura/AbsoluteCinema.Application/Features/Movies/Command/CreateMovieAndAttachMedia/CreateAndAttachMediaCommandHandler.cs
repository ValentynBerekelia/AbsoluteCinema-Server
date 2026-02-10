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
        MediaType Type,
        Stream? FileStream = null,
        string? FileName = null,
        string? ExternalUrl = null
    ) : IRequest<CreateAndAttachMediaResponse>;

    public record CreateAndAttachMediaResponse(Guid MovieId, Guid MediaId);

    public class CreateAndAttachMediaCommandHandler(
    IMovieRepository movies,
    IMediaRepository medias,
    IUnitOfWork unitOfWork,
    IStorageService storageService) : IRequestHandler<CreateAndAttachMediaCommand, CreateAndAttachMediaResponse>
    {
        private readonly IMovieRepository _movies = movies;
        private readonly IMediaRepository _medias = medias;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        private readonly IStorageService _storageService = storageService;

        public async Task<CreateAndAttachMediaResponse> Handle(
    CreateAndAttachMediaCommand command,
    CancellationToken ct)
        {
            var movieId = new MovieId(command.MovieId);
            var movie = await _movies.GetBySpecificationAsync(new MovieWithMediasSpec(movieId), ct)
                ?? throw new DomainException($"Movie with ID {command.MovieId} not found.");

            string finalUrl;

            if (command.FileStream != null)
            {
                var folder = MediaTypeExtensions.GetFolderName(command.Type);
                var extension = Path.GetExtension(command.FileName) ?? ".jpg";
                var cloudPath = $"{folder}/{command.MovieId}_{Guid.NewGuid()}{extension}";

                finalUrl = await _storageService.UploadImageAsync(command.FileStream, cloudPath);
            }
            else if (!string.IsNullOrWhiteSpace(command.ExternalUrl))
            {
                finalUrl = command.ExternalUrl;
            }
            else
            {
                throw new DomainException("Either a file or an external URL must be provided.");
            }

            var media = Media.Create(command.Type, finalUrl);
            await _medias.AddAsync(media, ct);
            movie.AddMedia(media);

            await _unitOfWork.SaveChangesAsync(ct);
            return new CreateAndAttachMediaResponse(command.MovieId, media.Id.Id);
        }
    }
}
