using AbsoluteCinema.Application.DTOs;
using AbsoluteCinema.Application.Repository;
using AbsoluteCinema.Domain.Enums;
using MediatR;

namespace AbsoluteCinema.Application.Features.Movies.Queries;

public class GetMoviesQueryHandler(IGetMoviesDtoQuery getMoviesDto)
    : IRequestHandler<GetMoviesQuery, GetMoviesResponse>
{
    private readonly IGetMoviesDtoQuery _getMoviesDto = getMoviesDto;

    public async Task<GetMoviesResponse> Handle(GetMoviesQuery request, CancellationToken ct)
    {
        var movieDtos = await _getMoviesDto.ExecuteAsync(request, ct);
        return new GetMoviesResponse(movieDtos);
    }
}

public record GetMoviesQuery : IRequest<GetMoviesResponse>
{
    public string? SearchTerm { get; init; }
    public List<string>? Genres { get; init; }
    
    //[first; second)
    public string? FirstDate { get; init; }
    public string? SecondDate { get; init; }
    
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
    
    public string? SortColumn { get; init; }
    public SortOrder SortOrder { get; init; } = SortOrder.Asc;
}

public record GetMoviesResponse(
    List<MovieDto> Movies
    ){}