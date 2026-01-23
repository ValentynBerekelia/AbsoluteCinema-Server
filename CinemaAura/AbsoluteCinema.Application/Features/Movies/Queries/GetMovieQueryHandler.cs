using AbsoluteCinema.Application.DTOs;
using AbsoluteCinema.Domain.Entities;
using MediatR;

namespace AbsoluteCinema.Application.Features.Movies.Queries;

public class GetMovieAdminQueryHandler : IRequestHandler<GetMovieQuery, GetMovieQueryResponse>
{
    private readonly IGetMovieDetailsQuery _detailsQuery;
    public async Task<GetMovieQueryResponse> Handle(GetMovieQuery request, CancellationToken ct)
    {
        return await _detailsQuery.ExecuteAsync(request, ct);
    }
}

public record GetMovieQuery : IRequest<GetMovieQueryResponse>
{
    public MovieId MovieId  { get; init; }
    public bool IsAdminsRequest { get; init; }
}

public record GetMovieQueryResponse(
    MovieId MovieId,
    string Description,
    decimal Rate,
    int AgeLimit,
    TimeSpan Duration,
    string Country,
    string Studio,
    string Language,
    IEnumerable<string> Genres,
    string? PosterUrl,
    IEnumerable<string> TrailerUrls,
    IEnumerable<string> ImageUrls,
    List<PersonDto> Persons)
{}

