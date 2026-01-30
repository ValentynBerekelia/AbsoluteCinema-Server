using AbsoluteCinema.Application.Features.Halls.Commands;
using AbsoluteCinema.Application.Features.Halls.Queries;
using AbsoluteCinema.Domain.Entities;
using AbsoluteCinema.Requests;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using static AbsoluteCinema.Application.Features.Halls.Commands.CreateHallCommandHandler;

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
        await _mediator.Send(command, ct);
        return NoContent();
    }
    
    [HttpGet]
    [Route("halls/{id:guid}")]
    public async Task<IActionResult> GetHall(Guid id,CancellationToken ct)
    {
        var query = new GetHallQuery(new HallId(id));
        var response = await _mediator.Send(query, ct);
        return Ok(response);
    }
    [HttpPost]
    [Route("admin/halls")]
    [ProducesResponseType(typeof(CreateHallResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> CreateHall([FromBody] CreateHallRequest request,CancellationToken ct)
    {
        var result = await _mediator.Send(new CreateHallCommand(request.Name), ct);
        return CreatedAtAction(nameof(GetHall), new { id = result.Id }, result);
    }
    [HttpPut]
    [Route("admin/halls/{hallId:guid}/seats/{seatId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateSeats([FromRoute] Guid hallId,[FromRoute] Guid seatId, [FromBody] UpdateSeatRequest request, CancellationToken ct)
    {
        await _mediator.Send(new UpdateSeatCommand(hallId, seatId,request.Row,request.Number,request.SeatTypeId), ct);
        return NoContent();
    }
    [HttpDelete]
    [Route("admin/hall/{hallId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteHall([FromRoute] Guid hallId,CancellationToken ct)
    {
        var command = new DeleteHallCommand(hallId);
        await _mediator.Send(command, ct);
        return NoContent();
    }
}