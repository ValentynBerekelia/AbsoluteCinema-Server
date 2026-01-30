using AbsoluteCinema.Application.DTOs;
using AbsoluteCinema.Domain.Entities;
using MediatR;

namespace AbsoluteCinema.Application.Features.Movies.Queries;

public class GetMovieQueryHandler(IGetMovieDetailsQuery detailsQuery)
    : IRequestHandler<GetMovieQuery, GetMovieQueryResponse>
{
    public async Task<GetMovieQueryResponse> Handle(GetMovieQuery request, CancellationToken ct)
    {
        var movie = await detailsQuery.ExecuteAsync(request, ct);
        if (movie is null)
        {
            throw new KeyNotFoundException($"Movie {request.MovieId} not found");
        }

        return movie;
    }
}

public record GetMovieQuery : IRequest<GetMovieQueryResponse>
{
    public MovieId MovieId  { get; }

    public GetMovieQuery(MovieId id)
    {
        MovieId = id;
    }
}

public record GetMovieQueryResponse(
    MovieId MovieId,
    string Title,
    string Description,
    decimal Rate,
    int AgeLimit,
    TimeSpan Duration,
    string Country,
    string Studio,
    string Language,
    IEnumerable<string> Genres,
    string? PosterUrl,
    string? BannerUrl,
    IEnumerable<string> TrailerUrls,
    IEnumerable<Guid> TrailerIds,
    IEnumerable<string> ImageUrls,
    IEnumerable<Guid> ImageIds,
    IEnumerable<PersonDto> Persons)
{}

