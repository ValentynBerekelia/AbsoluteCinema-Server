namespace AbsoluteCinema.Application.Features.Movies.Queries;

public interface IGetMovieDetailsQuery
{
    Task<GetMovieQueryResponse?> ExecuteAsync(GetMovieQuery query, CancellationToken ct);
}