using AbsoluteCinema.Application.DTOs;
using MediatR;

namespace AbsoluteCinema.Application.Features.Movies.Queries;

public class GetFeaturedMoviesQueryHandler(IGetFeaturedMoviesDtoQuery getFeaturedMoviesDto)
    : IRequestHandler<GetFeaturedMoviesQuery, GetFeaturedMoviesResponse>
{
    private readonly IGetFeaturedMoviesDtoQuery _getFeaturedMoviesDto = getFeaturedMoviesDto;

    public async Task<GetFeaturedMoviesResponse> Handle(GetFeaturedMoviesQuery request, CancellationToken ct)
    {
        var movies = await _getFeaturedMoviesDto.ExecuteAsync(ct);
        return new GetFeaturedMoviesResponse(movies);
    }
}

public record GetFeaturedMoviesQuery() : IRequest<GetFeaturedMoviesResponse>;

public record GetFeaturedMoviesResponse(
    List<MovieBannerDto> Movies
);