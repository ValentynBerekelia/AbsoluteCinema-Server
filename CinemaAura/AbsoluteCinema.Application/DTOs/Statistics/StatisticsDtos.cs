namespace AbsoluteCinema.Application.DTOs.Statistics;

// 1. Top movies
public record MoviePopularityDto(string MovieName, int TicketsSold, decimal TotalRevenue);

// 2. Hall occupancy
public record HallOccupancyDto(string HallName, double OccupancyPercentage, int TotalSeats, int SoldSeats);

// 3. Peak hours
public record PeakHourDto(DayOfWeek Day, int Hour, int Count);

// 4. Genre preferences
public record GenreStatDto(string GenreName, int TicketsSold, double Percentage);

public record DashboardStatsResponse(
    List<MoviePopularityDto> TopMovies,
    List<HallOccupancyDto> HallOccupancy,
    List<PeakHourDto> PeakHours,
    List<GenreStatDto> GenreStats
);