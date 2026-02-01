using AbsoluteCinema.Application.DTOs;
using AbsoluteCinema.Application.DTOs.Movie;
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
    Guid MovieId,
    string Title,
    string Description,
    decimal Rate,
    int AgeLimit,
    TimeSpan Duration,
    string Country,
    string Studio,
    string Language,
    IEnumerable<GenreDto> Genres,
    MediaDto? Poster,
    MediaDto? Banner,
    IEnumerable<MediaDto> Trailers,
    IEnumerable<MediaDto> Images,
    IEnumerable<PersonDto> Persons)
{}

