using AbsoluteCinema.Application.Repository;
using AbsoluteCinema.Domain.Entities;
using AbsoluteCinema.Domain.Exceptions;
using MediatR;

namespace AbsoluteCinema.Application.Features.Movies.Command.DeleteMovie;

public record DeleteMovieCommand(Guid MovieId) : IRequest<Unit>;

public class DeleteMovieCommandHandler(IMovieRepository movies, IUnitOfWork unitOfWork) : 
    IRequestHandler<DeleteMovieCommand, Unit>
{
    private readonly IMovieRepository _movies = movies;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Unit> Handle(DeleteMovieCommand command, CancellationToken ct)
    {
        var movieId = new MovieId(command.MovieId);
        var exists = await _movies.AnyAsync(movieId, ct);
        if (!exists)
        {
            throw new DomainException($"Movie with ID {command.MovieId} not found.");
        }

        await _movies.DeleteAsync(movieId, ct);
        await _unitOfWork.SaveChangesAsync(ct);

        return Unit.Value;
    }
}