using AbsoluteCinema.Application.Features.Sessions.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AbsoluteCinema.Controllers;

[ApiController]
[Route("api/sessions")]
public class SessionController : ControllerBase
{
    private readonly ISender _mediator;

    public SessionController(ISender mediator)
    {
        _mediator = mediator;
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateFull(Guid id, [FromBody] UpdateSessionFullCommand request)
    {
        var command = new UpdateSessionFullCommand(
            id,
            request.MovieId,
            request.HallId,
            request.Format,
            request.StartDateTime
        );

        await _mediator.Send(command);

        return NoContent();
    }

    [HttpPatch("{id:guid}")]
    public async Task<IActionResult> UpdatePartial(Guid id, [FromBody] UpdateSessionPartialCommand request)
    {
        var command = new UpdateSessionPartialCommand(
            id,
            request.MovieId,
            request.HallId,
            request.Format,
            request.StartDateTime
        );

        await _mediator.Send(command);

        return NoContent();
    }
}