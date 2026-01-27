using AbsoluteCinema.Application.DTOs;
using AbsoluteCinema.Application.Features.Sessions.Commands.CreateSession;
using AbsoluteCinema.Application.Features.Sessions.Commands.DeleteSession;
using AbsoluteCinema.Application.Features.Sessions.Commands.UpdateSession;
using AbsoluteCinema.Application.Features.Sessions.Queries;
using AbsoluteCinema.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AbsoluteCinema.Presentation.Controllers;

[Route("api")]
public class SessionController : ControllerBase
{
    private readonly ISender _sender;

    public SessionController(ISender sender)
    {
        _sender = sender;
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
        // конвертуємо рядок "Asc"/"Desc" в Enum (SortOrder)
        // 'true' означає ігнорувати регістр (asc == Asc == ASC)
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

        var result = await _sender.Send(query, ct);
        return Ok(result);
    }

    // 2. POST /api/admin/sessions
    [HttpPost("admin/sessions")]
    public async Task<IActionResult> CreateSession(
        [FromBody] AdminSessionCreateRequest request,
        CancellationToken ct)
    {
        var command = new CreateSessionCommand(
            request.MovieId,
            request.HallId,
            request.StartDateTime
        );

        var response = await _sender.Send(command, ct);
        return CreatedAtAction(nameof(GetSessionsByMovie), new { movieId = request.MovieId }, response);
    }

    // 3. PUT /api/admin/sessions/{sessionId}
    // Partial Update
    [HttpPut("sessions/{sessionId:guid}")]
    public async Task<IActionResult> UpdateSessionPartial(
        Guid sessionId,
        [FromBody] AdminSessionUpdatePartialRequest request,
        CancellationToken ct)
    {
        var command = new UpdateSessionCommand(
            sessionId,
            null,
            null,
            request.StartDateTime
        );

        await _sender.Send(command, ct);
        return NoContent();
    }

    // 4. PATCH /api/admin/sessions/{sessionId}
    // full update
    [HttpPatch("sessions/{sessionId:guid}")]
    public async Task<IActionResult> UpdateSessionFull(
        Guid sessionId,
        [FromBody] AdminSessionUpdateRequest request,
        CancellationToken ct)
    {
        var command = new UpdateSessionCommand(
            sessionId,
            request.MovieId,
            request.HallId,
            request.StartDateTime
        );

        await _sender.Send(command, ct);

        return NoContent();
    }

    // 5. DELETE /api/admin/sessions/{sessionId}
    [HttpDelete("sessions/{sessionId:guid}")]
    public async Task<IActionResult> DeleteSession(Guid sessionId, CancellationToken ct)
    {

        var command = new DeleteSessionCommand(sessionId);

        await _sender.Send(command, ct);

        return NoContent();
    }
}