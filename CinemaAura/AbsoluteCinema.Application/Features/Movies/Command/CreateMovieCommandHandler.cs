using AbsoluteCinema.Application.Repository;
using AbsoluteCinema.Domain.Entities;
using AbsoluteCinema.Domain.Exceptions;
using AbsoluteCinema.Domain.Specifications;
using MediatR;

namespace AbsoluteCinema.Application.Features.Movies.Command;

public class CreateMovieCommandHandler : IRequestHandler<CreateMovieCommand, CreateMovieResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMovieRepository _movies;

    public CreateMovieCommandHandler(IUnitOfWork unit, IMovieRepository movies)
    {
        _movies = movies;
        _unitOfWork = unit;
    }
    public async Task<CreateMovieResponse> Handle(CreateMovieCommand command, CancellationToken ct)
    {
        var movie = await _movies.GetBySpecificationAsync(new MovieByNameSpec(command.MovieName), ct);
        if (movie is not null)
        {
            throw new DomainException($"Movie {command.MovieName} already exists.");
        }
        var newMovie = Movie.Create(command.MovieName, command.Description, command.Rate, command.AgeLimit, command.Duration,
            command.Country, command.Studio, command.Language);
        await _movies.AddAsync(newMovie, ct);
        
        await _unitOfWork.SaveChangesAsync(ct);
        
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
    string Language
    ) : IRequest<CreateMovieResponse>{}

public record CreateMovieResponse(MovieId MovieId)
{
    
}

