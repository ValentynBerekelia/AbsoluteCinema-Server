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

    public async Task<DashboardStatsResponse> GetDashboardStatisticsAsync(DateTime from, DateTime to, CancellationToken ct = default)
    {
        // 1. Basic query (tickets per period)
        var ticketsQuery = _context.Tickets
            .AsNoTracking()
            .Include(t => t.Session)
            .ThenInclude(s => s.Movie)
            .ThenInclude(m => m.Genres)
            .Where(t => t.Status == TicketStatus.Confirmed &&
                        t.Session.StartDateTime >= from &&
                        t.Session.StartDateTime <= to);

        // 2. Top movies
        var topMovies = await ticketsQuery
            .GroupBy(t => t.Session.Movie.Name)
            .Select(g => new MoviePopularityDto(g.Key, g.Count(), 0))
            .OrderByDescending(x => x.TicketsSold)
            .Take(10)
            .ToListAsync(ct);

        // 3. Hall occupancy
        var sessionsData = await _context.Sessions
            .AsNoTracking()
            .Include(s => s.Hall)
            .Where(s => s.StartDateTime >= from && s.StartDateTime <= to)
            .Select(s => new
            {
                HallName = s.Hall.HallName,
                Capacity = s.Hall.SeatIds.Count,
                SoldTickets = s.Tickets.Count(t => t.Status == TicketStatus.Confirmed)
            })
            .ToListAsync(ct);

        var hallStats = sessionsData
            .GroupBy(s => s.HallName)
            .Select(g =>
            {
                var totalCapacity = g.Sum(x => x.Capacity);
                var totalSold = g.Sum(x => x.SoldTickets);
                return new HallOccupancyDto(
                    g.Key,
                    totalCapacity == 0 ? 0 : Math.Round((double)totalSold / totalCapacity * 100, 1),
                    totalCapacity,
                    totalSold
                );
            })
            .ToList();

        // 4. Peak hours
        var peakHours = await ticketsQuery
            .GroupBy(t => new { Day = t.Session.StartDateTime.DayOfWeek, Hour = t.Session.StartDateTime.Hour })
            .Select(g => new PeakHourDto(g.Key.Day, g.Key.Hour, g.Count()))
            .ToListAsync(ct);

        // 5. Genres
        var allSoldGenres = await ticketsQuery
            .SelectMany(t => t.Session.Movie.Genres.Select(g => g.Name))
            .ToListAsync(ct);

        var totalGenreCount = allSoldGenres.Count;
        var genreStats = allSoldGenres
            .GroupBy(g => g)
            .Select(g => new GenreStatDto(
                g.Key,
                g.Count(),
                totalGenreCount == 0 ? 0 : Math.Round((double)g.Count() / totalGenreCount * 100, 1)
            ))
            .OrderByDescending(x => x.TicketsSold)
            .ToList();

        return new DashboardStatsResponse(topMovies, hallStats, peakHours, genreStats);
    }
}