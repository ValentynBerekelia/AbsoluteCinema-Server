using AbsoluteCinema.Application.DTOs;

namespace AbsoluteCinema.Application.Features.Movies.Queries;

public interface IGetFeaturedMoviesDtoQuery
{
    Task<List<MovieBannerDto>> ExecuteAsync(CancellationToken ct);
}