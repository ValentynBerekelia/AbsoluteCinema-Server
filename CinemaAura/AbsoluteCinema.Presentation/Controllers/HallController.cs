using AbsoluteCinema.Application.Features.Halls.Commands;
using AbsoluteCinema.Domain.Entities;
using AbsoluteCinema.Requests;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AbsoluteCinema.Controllers;

[Route("api")]
[ApiController]
[ProducesResponseType(StatusCodes.Status500InternalServerError)]
public class HallController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpPost]
    [Route("admin/halls/seats")]
    [ProducesResponseType(typeof(AddSeatToHallResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> AddSeats([FromBody] AddSeatRequest request, CancellationToken ct)
    {
        var command = new AddSeatToHallCommand(
            request.HallId,
            new SeatTypeId(request.SeatTypeId),
            request.Seats.Adapt<IReadOnlyCollection<SeatInputModel>>()
        );

        var response = await _mediator.Send(command, ct);
        return Ok(response);
    }

    [HttpPut]
    [Route("admin/halls/{hallId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateHall([FromRoute] Guid hallId, [FromBody] UpdateHallRequest request, CancellationToken ct)
    {
        var command = new UpdateHallCommand(hallId, request.HallName);
        await _mediator.Send(command, ct);
        return NoContent();
    }

    [HttpDelete]
    [Route("admin/halls/{seatId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteSeat([FromRoute] Guid seatId, CancellationToken ct)
    {
        var command = new DeleteSeatCommand(seatId);
        var response = await _mediator.Send(command, ct);
        return Ok();
    }
}