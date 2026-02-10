using AbsoluteCinema.Application.DTOs.Statistics;
using MediatR;

namespace AbsoluteCinema.Application.Features.Statistics.Queries;

public record GetDashboardStatsQuery(DateTime? From = null, DateTime? To = null) : IRequest<DashboardStatsResponse>;