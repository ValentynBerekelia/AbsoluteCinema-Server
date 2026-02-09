using AbsoluteCinema.Application.DTOs;
using AbsoluteCinema.Application.Features.Movies.Queries;
using AbsoluteCinema.Domain.Entities;
using AbsoluteCinema.Domain.Enums;
using AbsoluteCinema.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace AbsoluteCinema.Infrastructure.EFQueries;

public class GetAdminMoviesStatsQuery(CinemaDbContext db) : IGetAdminMoviesStatsQuery
{
    private readonly CinemaDbContext _db = db;

    public async Task<GetAdminMovieStatsResponse> ExecuteAsync(GetAdminMovieStatsQueryRequest query, CancellationToken ct)
    {
        var today = DateTime.UtcNow.Date;

        var baseQuery = _db.Movies.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(query.SearchTerm))
        {
            baseQuery = baseQuery.Where(m => m.Name.ToLower().Contains(query.SearchTerm.ToLower()));
        }

        if (query.LastMovieId.HasValue)
        {
            var lastId = new MovieId(query.LastMovieId.Value);
            baseQuery = baseQuery.Where(m => m.Id.Id < lastId.Id);
        }

        var moviesData = await baseQuery
            .OrderByDescending(m => m.Id)
            .Take(query.PageSize)
            .Select(m => new
            {
                m.Id,
                m.Name,
                m.Duration,
                m.AgeLimit,
                PosterUrl = m.Medias.Where(c => c.Type == MediaType.PosterImage).Select(c => c.Url).FirstOrDefault() ?? ""
            })
            .ToListAsync(ct);

        if (!moviesData.Any())
            return new GetAdminMovieStatsResponse(new List<AdminMovieStatsDto>(), null);

        var movieIds = moviesData.Select(m => m.Id).ToList();

        var allSessions = await _db.Sessions
            .AsNoTracking()
            .Where(s => movieIds.Contains(s.MovieId) && s.StartDateTime >= today)
            .Select(s => new
            {
                s.MovieId,
                s.Id,
                s.StartDateTime,
                s.Format,
                TicketCount = s.Tickets.Count(),
                HallCapacity = _db.Seats.Count(seat => seat.HallId == s.HallId)
            })
            .ToListAsync(ct);

        var items = moviesData.Select(m =>
        {
            var movieSessions = allSessions.Where(s => s.MovieId == m.Id).ToList();

            return new AdminMovieStatsDto(
                m.Id.Id,
                m.Name,
                m.PosterUrl,
                m.Duration,
                m.AgeLimit,
                movieSessions.Sum(s => s.TicketCount),
                movieSessions.Sum(s => s.HallCapacity),
                movieSessions.Select(s => new SessionDto(s.Id.Id, s.StartDateTime, s.Format)).ToList()
            );
        }).ToList();

        var nextCursor = items.Count == query.PageSize ? items.Last().Id : (Guid?)null;

        return new GetAdminMovieStatsResponse(items, nextCursor);
    }
}