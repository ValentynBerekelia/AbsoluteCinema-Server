using AbsoluteCinema.Application.Features.Halls.Queries;
using AbsoluteCinema.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AbsoluteCinema.Controllers;
[Route("api")]
[ApiController]
public class HallController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;
    [HttpGet]
    [Route("halls")]
    public async Task<IActionResult> GetHalls(CancellationToken ct)
    {
        var response = await _mediator.Send(new GetHallsQuery(),ct);
        return Ok(response);
    }

    [HttpGet]
    [Route("halls/{id:guid}")]
    public async Task<IActionResult> GetHall(Guid id,CancellationToken ct)
    {
        var query = new GetHallQuery(new HallId(id));
        var response = await _mediator.Send(query, ct);
        return Ok(response);
    }
}