using AbsoluteCinema.Application.DTOs;
using AbsoluteCinema.Application.DTOs.Movie;
using AbsoluteCinema.Application.Features.Movies.Queries;
using AbsoluteCinema.Domain.Entities;
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
                m.Id.Id,
                m.Name,
                m.Description,
                m.Rate,
                m.AgeLimit,
                m.Duration,
                m.Country,
                m.Studio,
                m.Language,
                m.Genres.Select(g => new GenreDto(g.Id.Id, g.Name)),
                m.Medias
                    .Where(c => c.Type == MediaType.PosterImage)
                    .Select(c => new MediaDto(c.Id.Id, c.Type ,c.Url) )
                    .FirstOrDefault(),
                m.Medias
                    .Where(c => c.Type == MediaType.BannerImage)
                    .Select(c => new MediaDto(c.Id.Id, c.Type ,c.Url))
                    .FirstOrDefault(),
                m.Medias
                    .Where(c => c.Type == MediaType.Video)
                    .Select(c => new MediaDto(c.Id.Id, c.Type ,c.Url)),
                m.Medias
                    .Where(c => c.Type == MediaType.Image)
                    .Select(c => new MediaDto(c.Id.Id, c.Type ,c.Url)),
                m.Persons
                    .Select(p=> new PersonDto(
                        p.Id.Id,
                        p.Name,
                        p.PersonRole,
                        p.Media != null ? p.Media.Url : null
                        ))
                
            ));

        return await movie.FirstOrDefaultAsync(ct);
    }
}