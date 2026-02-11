namespace AbsoluteCinema.Application.Features.Movies.Queries;
public interface IGetMovieRecommendationDtoQuery
{
    Task<GetMovieRecommendationResponse> ExecuteAsync(GetMovieRecommendationQuery query, CancellationToken ct);
}