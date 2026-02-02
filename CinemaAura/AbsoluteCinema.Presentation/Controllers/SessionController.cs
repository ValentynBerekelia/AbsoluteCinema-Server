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
