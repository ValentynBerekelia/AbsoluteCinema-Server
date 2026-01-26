using AbsoluteCinema.Application.Features.Sessions.Commands.DeleteSession;
using AbsoluteCinema.Domain.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AbsoluteCinema.Controllers;

[Route("api")]
public class SessionController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpDelete]
    [Route("admin/sessions/{id:guid}")]
    public async Task<IActionResult> DeleteSession(Guid id, CancellationToken ct)
    {
        try
        {
            await _mediator.Send(new DeleteSessionCommand(id), ct);
            return NoContent();
        }
        catch (DomainException ex) when (ex.Message.Contains("not found", StringComparison.OrdinalIgnoreCase))
        {
            return NotFound(new { error = ex.Message, details = "The session with the specified ID does not exist." });
        }
    }
}