using AbsoluteCinema.Application.Repository;
using AbsoluteCinema.Domain.Entities;
using AbsoluteCinema.Domain.Exceptions;
using AbsoluteCinema.Domain.Specifications;
using MediatR;

namespace AbsoluteCinema.Application.Features.Movies.Command;

public class CreateMovieCommandHandler(IUnitOfWork unit, IMovieRepository movies, ICreateGenreCommend createGenre)
    : IRequestHandler<CreateMovieCommand, CreateMovieResponse>
{
    private readonly ICreateGenreCommend _createGenre = createGenre;

    public async Task<CreateMovieResponse> Handle(CreateMovieCommand command, CancellationToken ct)
    {
        var movie = await movies.GetBySpecificationAsync(new MovieByNameSpec(command.MovieName), ct);
        if (movie is not null)
        {
            throw new DomainException($"Movie {command.MovieName} already exists.");
        }
        
        var genres = await _createGenre.ExecuteAsync(command.Genres, ct);

        var newMovie = Movie.Create(command.MovieName, command.Description, command.Rate, command.AgeLimit, command.Duration,
            command.Country, command.Studio, command.Language);

        foreach (var g in genres)
        {
            newMovie.AddGenre(g);
        }

        await movies.AddAsync(newMovie, ct);
        
        await unit.SaveChangesAsync(ct);
        
        return new CreateMovieResponse(newMovie.Id);
    }
}

public record  CreateMovieCommand(
    string MovieName,
    string Description,
    decimal Rate,
    int AgeLimit,
    TimeSpan Duration,
    string Country,
    string Studio,
    string Language,
    List<string> Genres
    ) : IRequest<CreateMovieResponse>{}

public record CreateMovieResponse(MovieId MovieId)
{
    
}