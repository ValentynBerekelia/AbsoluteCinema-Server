using AbsoluteCinema.Application.Repository;
using AbsoluteCinema.Domain.Entities;
using AbsoluteCinema.Domain.Exceptions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using static AbsoluteCinema.Application.Features.Genres.Commands.AttachGenreToMovie.AttachGenreToMovieCommandHandler;

namespace AbsoluteCinema.Application.Features.Genres.Commands.AttachGenreToMovie
{
    public class AttachGenreToMovieCommandHandler(
        IGenreRepository genreRepository,
        IMovieRepository movieRepository,
        IUnitOfWork unitOfWork)
        : IRequestHandler<AttachGenreToMovieCommand,Unit>
    {
        private readonly IGenreRepository _genre = genreRepository;
        private readonly IMovieRepository _movie = movieRepository;
        private readonly IUnitOfWork _unit = unitOfWork;
        public async Task<Unit> Handle(AttachGenreToMovieCommand command,CancellationToken ct)
        {
            var genreId = new GenreId(command.GenreId);
            var movieId = new MovieId(command.MovieId);

            var genre = await _genre.GetByIdAsync(genreId, ct);
            if(genre is null)
            {
                throw new DomainException($"Genre with id {command.GenreId} not found.");
            }

            var movie = await _movie.GetByIdAsync(movieId, ct);
            if(movie is null)
            {
                throw new DomainException($"Movie with id {command.MovieId} not found.");
            }
            movie.AddGenre(genre);
            _movie.Update(movie);
            await _unit.SaveChangesAsync(ct);
            return Unit.Value;
        }
        public record AttachGenreToMovieCommand(
            Guid MovieId,
            Guid GenreId
            ) :IRequest<Unit>;
    }
}
