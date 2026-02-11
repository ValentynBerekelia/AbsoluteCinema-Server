using AbsoluteCinema.Application.DTOs.Statistics;
using AbsoluteCinema.Application.Repository;
using MediatR;

namespace AbsoluteCinema.Application.Features.Statistics.Queries;

public class GetDashboardStatsHandler : IRequestHandler<GetDashboardStatsQuery, DashboardStatsResponse>
{
    private readonly IStatisticsRepository _repository;

    public GetDashboardStatsHandler(IStatisticsRepository repository)
    {
        _repository = repository;
    }

    public async Task<DashboardStatsResponse> Handle(GetDashboardStatsQuery request, CancellationToken cancellationToken)
    {
        var fromDate = request.From ?? DateTime.UtcNow.AddMonths(-1);
        var toDate = request.To ?? DateTime.UtcNow;

        return await _repository.GetDashboardStatisticsAsync(fromDate, toDate, cancellationToken);
    }
}