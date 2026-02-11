using AbsoluteCinema.Application.DTOs.Statistics;
using AbsoluteCinema.Application.Features.Statistics.Queries;
using AbsoluteCinema.Infrastructure.Security;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AbsoluteCinema.Presentation.Controllers.Admin;

[ApiController]
[Route("api/admin/analytics")]
[Authorize(Policy = Permissions.TicketsManage)]
public sealed class AnalyticsController : ControllerBase
{
    private readonly IMediator _mediator;

    public AnalyticsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("dashboard")]
    [ProducesResponseType(typeof(DashboardStatsResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<DashboardStatsResponse>> GetDashboardStatistics(
        [FromQuery] DateTime from,
        [FromQuery] DateTime to,
        CancellationToken ct = default)
    {
        var result = await _mediator.Send(
            new GetDashboardStatsQuery(from, to),
            ct);

        return Ok(result);
    }
}