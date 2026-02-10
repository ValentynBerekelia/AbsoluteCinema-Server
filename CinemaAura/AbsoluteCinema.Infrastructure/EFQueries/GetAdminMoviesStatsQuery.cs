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
            var search = query.SearchTerm.ToLower();
            baseQuery = baseQuery.Where(m => m.Name.ToLower().Contains(search));
        }

        if (query.LastMovieId.HasValue)
        {
            var lastIdGuid = query.LastMovieId.Value;

            baseQuery = baseQuery.Where(m => (Guid)(object)m.Id < lastIdGuid);
        }

        var moviesData = await baseQuery
            .OrderByDescending(m => m.Id)
            .Take(query.PageSize + 1)
            .Select(m => new
            {
                m.Id,
                m.Name,
                m.Duration,
                m.AgeLimit,
                PosterUrl = m.Medias
                    .Where(c => c.Type == MediaType.PosterImage)
                    .Select(c => c.Url)
                    .FirstOrDefault() ?? ""
            })
            .ToListAsync(ct);

        if (!moviesData.Any())
            return new GetAdminMovieStatsResponse(new List<AdminMovieStatsDto>(), null);

        bool hasNextPage = moviesData.Count > query.PageSize;
        Guid? nextCursor = null;

        if (hasNextPage)
        {
            nextCursor = moviesData[moviesData.Count - 2].Id.Id;
            moviesData.RemoveAt(moviesData.Count - 1);
        }

        var movieIds = moviesData.Select(m => m.Id).ToList();

        var hallCapacities = await _db.Seats
            .AsNoTracking()
            .GroupBy(s => s.HallId)
            .Select(g => new { HallId = g.Key, Capacity = g.Count() })
            .ToDictionaryAsync(x => x.HallId, x => x.Capacity, ct);

        var sessionsFromDb = await _db.Sessions
            .AsNoTracking()
            .Where(s => movieIds.Contains(s.MovieId) && s.StartDateTime >= today)
            .Select(s => new
            {
                s.MovieId,
                s.Id,
                s.StartDateTime,
                s.Format,
                s.HallId,
                TicketCount = s.Tickets.Count()
            })
            .ToListAsync(ct);

        var items = moviesData.Select(m =>
        {
            var movieSessions = sessionsFromDb
                .Where(s => s.MovieId == m.Id)
                .Select(s => new
                {
                    s.Id,
                    s.StartDateTime,
                    s.Format,
                    s.TicketCount,
                    HallCapacity = hallCapacities.GetValueOrDefault(s.HallId, 0)
                })
                .ToList();

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

        return new GetAdminMovieStatsResponse(items, nextCursor);
    }
}