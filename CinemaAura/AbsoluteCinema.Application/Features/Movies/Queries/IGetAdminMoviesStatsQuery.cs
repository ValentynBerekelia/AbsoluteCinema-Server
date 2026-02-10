using AbsoluteCinema.Application.DTOs;

namespace AbsoluteCinema.Application.Features.Movies.Queries;
public interface IGetAdminMoviesStatsQuery
{
    Task<GetAdminMovieStatsResponse> ExecuteAsync(GetAdminMovieStatsQueryRequest query,CancellationToken ct);
}