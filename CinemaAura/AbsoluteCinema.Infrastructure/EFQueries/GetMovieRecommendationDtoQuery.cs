using AbsoluteCinema.Application.DTOs;
using AbsoluteCinema.Application.Features.Movies.Queries;
using AbsoluteCinema.Domain.Entities;
using AbsoluteCinema.Domain.Enums;
using AbsoluteCinema.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace AbsoluteCinema.Infrastructure.EFQueries;



public class GetMovieRecommendationDtoQuery(CinemaDbContext db) : IGetMovieRecommendationDtoQuery
{
    private readonly CinemaDbContext _db = db;

    public async Task<GetMovieRecommendationResponse> ExecuteAsync(GetMovieRecommendationQuery query, CancellationToken ct)
    {
        var currentMovieId = new MovieId(query.MovieId);

        var genresIds = await _db.Movies
            .Where(m => m.Id == currentMovieId)
            .SelectMany(m => m.Genres.Select(g => g.Id))
            .ToListAsync(ct);

        var moviesQuery = _db.Movies
            .AsNoTracking()
            .Where(m => m.Id != currentMovieId);

        if (genresIds.Any())
        {
            moviesQuery = moviesQuery
                .Where(m => m.Genres.Any(g => genresIds.Contains(g.Id)))
                .OrderByDescending(m => m.Genres.Count(g => genresIds.Contains(g.Id)))
                .ThenByDescending(m => m.Rate);
        }
        else
        {
            moviesQuery = moviesQuery.OrderByDescending(m => m.Rate);
        }

        var movies = await moviesQuery
            .Take(query.Limit)
            .Select(m => new MovieRecommendationDto(
                m.Id.Id,
                m.Name,
                m.Medias
                    .Where(media => media.Type == MediaType.PosterImage)
                    .Select(media => media.Url)
                    .FirstOrDefault() ?? ""
            ))
            .ToListAsync(ct);

        return new GetMovieRecommendationResponse(movies);
    }
}