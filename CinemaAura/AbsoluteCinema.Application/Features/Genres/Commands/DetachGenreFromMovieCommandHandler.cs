using AbsoluteCinema.Application.Repository;
using AbsoluteCinema.Domain.Entities;
using AbsoluteCinema.Domain.Exceptions;
using AbsoluteCinema.Domain.Specifications;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbsoluteCinema.Application.Features.Genres.Commands
{
    public class DetachGenreFromMovieCommandHandler(IMovieRepository moviesRepository,IUnitOfWork unitOfWork)
        : IRequestHandler<DetachGenreFromMovieCommand,Unit>
    {
        private readonly IMovieRepository _movies = moviesRepository;
        private readonly IUnitOfWork _unit = unitOfWork;
        public async Task<Unit> Handle(DetachGenreFromMovieCommand command,CancellationToken ct)
        {
            var movieId = new MovieId(command.MovieId);
            var genreId = new GenreId(command.GenreId);

            var movie = await _movies.GetBySpecificationAsync(new MovieIncludeGenresSpec(movieId),ct)
                ?? throw new DomainException($"Movie with id {command.MovieId} not found.");
            if(!movie.Genres.Any(g=>g.Id == genreId))
            {
                throw new DomainException($"Media with ID {command.GenreId} is not attached to movie {command.MovieId}");
            }
            movie.RemoveGenre(genreId);
            _movies.Update(movie);

            await _unit.SaveChangesAsync(ct);
            return Unit.Value;
        }
    }
    public record DetachGenreFromMovieCommand(
        Guid MovieId,
        Guid GenreId
        ) :IRequest<Unit>;
}
