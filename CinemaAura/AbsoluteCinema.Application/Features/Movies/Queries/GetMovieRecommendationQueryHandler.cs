using AbsoluteCinema.Application.DTOs;
using MediatR;

namespace AbsoluteCinema.Application.Features.Movies.Queries;

public class GetMovieRecommendationQueryHandler(IGetMovieRecommendationDtoQuery getMovieRecommendationDtoQuery)
    : IRequestHandler<GetMovieRecommendationQuery, GetMovieRecommendationResponse>
{
    private readonly IGetMovieRecommendationDtoQuery _getMovieRecommendationDtoQuery = getMovieRecommendationDtoQuery;
    public Task<GetMovieRecommendationResponse> Handle(GetMovieRecommendationQuery request, CancellationToken cancellationToken)
    {
        var movies = _getMovieRecommendationDtoQuery.ExecuteAsync(request, cancellationToken);

        return movies;
    }
}

public record GetMovieRecommendationQuery(
    Guid MovieId,
    int Limit = 10
) : IRequest<GetMovieRecommendationResponse>;

public record GetMovieRecommendationResponse (
    List<MovieRecommendationDto> Movies
);