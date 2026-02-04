using AbsoluteCinema.Application.Repository;
using AbsoluteCinema.Domain.Entities;
using AbsoluteCinema.Domain.Exceptions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbsoluteCinema.Application.Features.Genres.Commands
{
    public class CreateGenreToMovieCommandHandler(
        IMovieRepository movieRepository,
        IGenreRepository genreRepository,
        IUnitOfWork unitOfWork) : IRequestHandler<CreateGenreToMovieCommand, CreateGenreToMovieResponse>
    {
        private readonly IMovieRepository _movie = movieRepository;
        private readonly IGenreRepository _genre = genreRepository;
        private readonly IUnitOfWork _unit = unitOfWork;
        public async Task<CreateGenreToMovieResponse> Handle(CreateGenreToMovieCommand command, CancellationToken ct)
        {
            var genre = Genre.Create(command.GenreName);
            await _genre.AddAsync(genre, ct);


            var movieId = new MovieId(command.MovieId);
            var movie = await _movie.GetByIdAsync(movieId, ct);
            if(movie is null)
            {
                throw new DomainException($"Movie with id {command.MovieId} not found.");
            }
            movie.AddGenre(genre);
            _movie.Update(movie);
            await _unit.SaveChangesAsync(ct);
            
            return new CreateGenreToMovieResponse(genre.Id.Id, genre.Name);
        }
    }
    public record CreateGenreToMovieCommand(
        Guid MovieId,
        string GenreName): IRequest<CreateGenreToMovieResponse>;
    public record CreateGenreToMovieRequest(string Name);
    
    public record CreateGenreToMovieResponse(Guid GenreId,string Name);
}
