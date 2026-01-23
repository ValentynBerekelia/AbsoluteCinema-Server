using AbsoluteCinema.Application.DTOs;
using AbsoluteCinema.Application.Features.Movies.Queries;
using AbsoluteCinema.Domain.Enums;
using AbsoluteCinema.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace AbsoluteCinema.Infrastructure.EFQueries;

public class GetMovieDetailsQuery(CinemaDbContext db) : IGetMovieDetailsQuery
{
    private readonly CinemaDbContext _db = db;

    public async Task<GetMovieQueryResponse?> ExecuteAsync(GetMovieQuery query, CancellationToken ct)
    {
        var movie = _db.Movies
            .AsNoTracking()
            .Where(m => m.Id == query.MovieId)
            .Select(m => new GetMovieQueryResponse(
                m.Id,
                m.Description,
                m.Rate,
                m.AgeLimit,
                m.Duration,
                m.Country,
                m.Studio,
                m.Language,
                m.Genres.Select(g => g.Name),
                m.Medias
                    .Where(c => c.Type == MediaType.BannerImage)
                    .Select(c => c.Url)
                    .FirstOrDefault(),
                m.Medias
                    .Where(c => c.Type == MediaType.Video)
                    .Select(c => c.Url),
                m.Medias
                    .Where(c => c.Type == MediaType.Image)
                    .Select(c => c.Url),
                m.Persons
                    .Select(p=> new PersonDto(
                        p.Id,
                        p.Name,
                        p.PersonRole,
                        p.Media.Url
                        ))
            ));

        return await movie.FirstOrDefaultAsync(ct);
    }
}