using AbsoluteCinema.Application.DTOs;
using MediatR;

namespace AbsoluteCinema.Application.Features.Movies.Queries;


public class GetAdminMovieStatsQueryHandler(IGetAdminMoviesStatsQuery getAdminMoviesStatsQuery) 
    : IRequestHandler<GetAdminMovieStatsQueryRequest, GetAdminMovieStatsResponse>
{
    private readonly IGetAdminMoviesStatsQuery _getAdminMoviesStatsQuery = getAdminMoviesStatsQuery;

    public async Task<GetAdminMovieStatsResponse> Handle(GetAdminMovieStatsQueryRequest request, CancellationToken cancellationToken)
    {
        var movies = await _getAdminMoviesStatsQuery.ExecuteAsync(request, cancellationToken);
        
        return movies;
    }
}

public record GetAdminMovieStatsQueryRequest(
    Guid? LastMovieId = null,
    int PageSize = 10,
    string? SearchTerm = null
) : IRequest<GetAdminMovieStatsResponse>;

public record GetAdminMovieStatsResponse(
    List<AdminMovieStatsDto> Movies,
    Guid? NextCursor
    );