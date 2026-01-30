using AbsoluteCinema.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using AbsoluteCinema.Application.DTOs;
using AbsoluteCinema.Domain.Enums;
using AbsoluteCinema.Application.Features.Movies.Queries;

namespace AbsoluteCinema.Infrastructure.EFQueries;

public class GetFeaturedMoviesDtoQuery(CinemaDbContext db) : IGetFeaturedMoviesDtoQuery
{
    private readonly CinemaDbContext _db = db;

    public async Task<List<MovieBannerDto>> ExecuteAsync(CancellationToken ct)
    {
        var today = DateTime.UtcNow.Date;

        return await _db.Movies
            .AsNoTracking()
            .Where(m => m.Medias.Any(me => me.Type == MediaType.BannerImage))
            .Where(m => _db.Sessions.Any(s => s.MovieId == m.Id && s.StartDateTime >= today))
            .OrderByDescending(m => m.Rate)
            .Take(5)
            .Select(m => new MovieBannerDto(
                m.Id,
                m.Name,
                m.Medias
                    .Where(c => c.Type == MediaType.BannerImage)
                    .Select(j => j.Url)
                    .FirstOrDefault(),
                _db.Sessions
                    .Where(s => s.MovieId == m.Id && s.StartDateTime >= today && s.StartDateTime < today.AddDays(1))
                    .OrderBy(s => s.StartDateTime)
                    .Select(s => new SessionShortDto(s.Id, s.StartDateTime))
                    .ToList()
            ))
            .ToListAsync(ct);
    }
}