using AbsoluteCinema.Application.DTOs;
using AbsoluteCinema.Domain.Entities;

namespace AbsoluteCinema.Application.Features.Movies.Queries;

public interface IGetMoviesDtoQuery
{
    Task<List<MovieDto>> ExecuteAsync(GetMoviesQuery query, CancellationToken ct);
}