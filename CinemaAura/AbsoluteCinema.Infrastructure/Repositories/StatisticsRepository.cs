using AbsoluteCinema.Application.DTOs.Statistics;
using AbsoluteCinema.Application.Repository;
using AbsoluteCinema.Domain.Entities;
using AbsoluteCinema.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace AbsoluteCinema.Infrastructure.Repositories;

public class StatisticsRepository : IStatisticsRepository
{
    private readonly CinemaDbContext _context;

    public StatisticsRepository(CinemaDbContext context)
    {
        _context = context;
    }

    public async Task<DashboardStatsResponse> GetDashboardStatisticsAsync(
    DateTime to = default,
    DateTime from = default,


    CancellationToken ct = default)
    {
        var nowUtc = DateTime.UtcNow;

        if (from == default)
        {
            from = DateTime.UtcNow.AddMonths(-1).Date;
        }
        if (to == default)
        {
            to = DateTime.UtcNow.Date;
        }
        var toInclusive = to;
        var fromInclusive = from;

        var soldStatus = TicketStatus.Confirmed;

        var ticketsQuery = _context.Tickets
            .AsNoTracking()
            .Where(t =>
                t.Status == soldStatus &&
                t.Session.StartDateTime >= fromInclusive &&
                t.Session.StartDateTime <= toInclusive
            );

        var topMovieAggregates = await ticketsQuery
            .Select(t => new
            {
                MovieId = t.Session.MovieId,
                t.SessionId,
                SeatTypeId = t.Seat.SeatTypeId
            })
            .GroupBy(x => x.MovieId)
            .Select(g => new
            {
                MovieId = g.Key,
                TicketsSold = g.Count(),
                Revenue = g.Sum(x =>
                    _context.TypePrices
                        .Where(tp =>
                            tp.SessionId == x.SessionId &&
                            tp.SeatTypeId == x.SeatTypeId
                        )
                        .Select(tp => tp.Price)
                        .FirstOrDefault()
                )
            })
            .OrderByDescending(x => x.TicketsSold)
            .Take(10)
            .ToListAsync(ct);

        var topMovieIds = topMovieAggregates
            .Select(x => x.MovieId)
            .ToList();

        var movies = await _context.Movies
            .AsNoTracking()
            .Where(m => topMovieIds.Contains(m.Id))
            .Select(m => new { m.Id, m.Name })
            .ToListAsync(ct);

        var topMovies = topMovieAggregates
            .Select(x =>
            {
                var movie = movies.FirstOrDefault(m => m.Id.Equals(x.MovieId));
                var name = movie is null ? string.Empty : movie.Name.ToString();

                return new MoviePopularityDto(
                    name,
                    x.TicketsSold,
                    x.Revenue
                );
            })
            .ToList();

        var sessionsData = await _context.Sessions
            .AsNoTracking()
            .Where(s => s.StartDateTime >= fromInclusive && s.StartDateTime <= toInclusive)
            .Select(s => new
            {
                s.Id,
                HallId = s.Hall.Id,
                HallName = s.Hall.HallName
            })
            .ToListAsync(ct);

        var sessionIds = sessionsData
            .Select(x => x.Id)
            .ToList();

        var soldTicketsPerSession = await _context.Tickets
            .AsNoTracking()
            .Where(t => sessionIds.Contains(t.SessionId) && t.Status == soldStatus)
            .GroupBy(t => t.SessionId)
            .Select(g => new
            {
                SessionId = g.Key,
                SoldTickets = g.Count()
            })
            .ToListAsync(ct);

        var hallCapacities = await _context.Seats
            .AsNoTracking()
            .GroupBy(s => s.HallId)
            .Select(g => new
            {
                HallId = g.Key,
                Capacity = g.Count()
            })
            .ToListAsync(ct);

        var hallStats = sessionsData
            .GroupBy(x => new { x.HallId, x.HallName })
            .Select(g =>
            {
                var capacity = hallCapacities
                    .Where(x => x.HallId == g.Key.HallId)
                    .Select(x => x.Capacity)
                    .FirstOrDefault();

                var totalSold = g.Sum(s =>
                    soldTicketsPerSession
                        .Where(x => x.SessionId == s.Id)
                        .Select(x => x.SoldTickets)
                        .FirstOrDefault()
                );

                var totalCapacity = capacity * g.Count();

                return new HallOccupancyDto(
                    g.Key.HallName,
                    totalCapacity == 0 ? 0 : Math.Round((double)totalSold / totalCapacity * 100, 1),
                    totalCapacity,
                    totalSold
                );
            })
            .ToList();

        var peakHours = await ticketsQuery
            .GroupBy(t => new
            {
                Day = t.Session.StartDateTime.DayOfWeek,
                Hour = t.Session.StartDateTime.Hour
            })
            .Select(g => new PeakHourDto(
                g.Key.Day,
                g.Key.Hour,
                g.Count()
            ))
            .ToListAsync(ct);

        var allSoldGenres = await ticketsQuery
            .SelectMany(t => t.Session.Movie.Genres.Select(g => g.Name))
            .ToListAsync(ct);

        var totalGenreCount = allSoldGenres.Count;

        var genreStats = allSoldGenres
            .GroupBy(g => g)
            .Select(g => new GenreStatDto(
                g.Key.ToString(),
                g.Count(),
                totalGenreCount == 0 ? 0 : Math.Round((double)g.Count() / totalGenreCount * 100, 1)
            ))
            .OrderByDescending(x => x.TicketsSold)
            .ToList();

        return new DashboardStatsResponse(topMovies, hallStats, peakHours, genreStats);
    }
    private static DateTime ToUtc(DateTime dt)
    {
        return dt.Kind switch
        {
            DateTimeKind.Utc => dt,
            DateTimeKind.Local => dt.ToUniversalTime(),
            DateTimeKind.Unspecified => DateTime.SpecifyKind(dt, DateTimeKind.Utc),
            _ => DateTime.SpecifyKind(dt, DateTimeKind.Utc)
        };
    }

}