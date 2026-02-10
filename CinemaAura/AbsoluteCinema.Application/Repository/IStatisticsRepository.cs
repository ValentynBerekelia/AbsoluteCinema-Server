using AbsoluteCinema.Application.DTOs.Statistics;

namespace AbsoluteCinema.Application.Repository;

public interface IStatisticsRepository
{
    Task<DashboardStatsResponse> GetDashboardStatisticsAsync(DateTime from, DateTime to, CancellationToken ct = default);
}