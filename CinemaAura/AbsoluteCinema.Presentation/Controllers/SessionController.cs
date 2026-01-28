using AbsoluteCinema.Application.DTOs;
using AbsoluteCinema.Application.Features.Sessions.Commands;
using AbsoluteCinema.Application.Features.Sessions.Commands.CreateSession;
using AbsoluteCinema.Application.Features.Sessions.Commands.DeleteSession;
using AbsoluteCinema.Application.Features.Sessions.Queries;
using AbsoluteCinema.Domain.Entities;
using AbsoluteCinema.Domain.Enums;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AbsoluteCinema.Controllers;

[ApiController]
[Route("api")]
public class SessionController : ControllerBase
{
    private readonly IMediator _mediator;

    public SessionController(IMediator mediator)
    {
        _mediator = mediator;
    }
    // 1. GET /api/admin/movies/{movieId}/sessions
    [HttpGet("movies/{movieId:guid}/sessions")]
    public async Task<IActionResult> GetSessionsByMovie(
        Guid movieId,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string sortColumn = "startDateTime",
        [FromQuery] string sortOrder = "Asc",
        CancellationToken ct = default)
    {
        if (!Enum.TryParse<SortOrder>(sortOrder, true, out var parsedSortOrder))
        {
            parsedSortOrder = SortOrder.Asc;
        }

        var query = new GetSessionsListQuery
        {
            MovieId = movieId,
            PageNumber = pageNumber,
            PageSize = pageSize,
            SortColumn = sortColumn,
            SortOrder = parsedSortOrder
        };

        var result = await _mediator.Send(query, ct);
        return Ok(result);
    }

    [HttpPost("admin/sessions")]
    public async Task<IActionResult> CreateSession(
       [FromBody] CreateSessionCommand request,
       CancellationToken ct)
    {
        var command = new CreateSessionCommand(
            request.MovieId,
            request.HallId,
            request.Format,
            request.StartTime,
            request.Prices
        );

        var response = await _mediator.Send(command, ct);
        return CreatedAtAction(nameof(GetSessionsByMovie), new { movieId = request.MovieId }, response);
    }

    [HttpPut("admin/sessions/{id:guid}")]
    public async Task<IActionResult> UpdateFull(Guid id, [FromBody] SessionUpdateFullRequest request)
    {
        var command = request.Adapt<UpdateSessionFullCommand>();
        var finalCommand = command with { Id = new SessionId(id) };

        await _mediator.Send(finalCommand);
        return NoContent();
    }

    [HttpPatch("admin/sessions/{id:guid}")]
    public async Task<IActionResult> UpdatePartial(Guid id, [FromBody] SessionUpdatePartialRequest request)
    {
        var command = request.Adapt<UpdateSessionPartialCommand>();
        var finalCommand = command with { Id = new SessionId(id) };

        await _mediator.Send(finalCommand);
        return NoContent();
    }

    [HttpDelete("sessions/{sessionId:guid}")]
    public async Task<IActionResult> DeleteSession(Guid sessionId, CancellationToken ct)
    {

        var command = new DeleteSessionCommand(sessionId);

        await _mediator.Send(command, ct);

        return NoContent();
    }
}