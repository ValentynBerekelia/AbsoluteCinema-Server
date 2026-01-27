using AbsoluteCinema.Application.DTOs;
using AbsoluteCinema.Application.Features.Sessions.Commands;
using AbsoluteCinema.Domain.Entities;
using Mapster;
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
}