using AbsoluteCinema.Application.Features.Halls.Queries;
using AbsoluteCinema.Application.Features.Sessions.Queries;
using AbsoluteCinema.Application.Features.Tickets.Commands;
using AbsoluteCinema.Application.Features.Tickets.Queries;
using AbsoluteCinema.Domain.Entities;
using AbsoluteCinema.Infrastructure.EFQueries;
using AbsoluteCinema.Requests;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using static AbsoluteCinema.Application.Features.Tickets.Commands.CreateTicketCommandHandler;
namespace AbsoluteCinema.Controllers
{
    [ApiController]
    [Route("api")]
    public class TicketController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpGet]
        [Route("ticket{ticketId:guid}")]
        public async Task<IActionResult> GetTicket(Guid ticketId, CancellationToken ct)
        {
            var query = new GetTicketQuery(new TicketId(ticketId));
            var response = await _mediator.Send(query, ct);
            return Ok(response);
        }
        [HttpPost]
        [Route("ticket")]
        public async Task<IActionResult> CreateTicket([FromBody] CreateTicketCommand request, CancellationToken ct)
        {
            var command = new CreateTicketCommand(
                request.SessionId,
                request.SeatId,
                request.UserId
            );
            var response = await _mediator.Send(command, ct);
            return Ok(response);
        }

        [HttpPatch]
        [Route("ticket/{ticketId:guid}")]
        public async Task<IActionResult> UpdateTicket([FromRoute] Guid ticketId, [FromBody] UpdateTicketRequest request, CancellationToken ct)
        {
            var command = new UpdateTicketCommand(ticketId, request.SessionId, request.SeatId, request.UserId);
            await _mediator.Send(command, ct);
            return NoContent();
        }

        [HttpDelete]
        [Route("ticket/{ticketId:guid}")]
        public async Task<IActionResult> DeleteTicket([FromRoute] Guid ticketId, CancellationToken ct)
        {
            var command = new DeleteTicketCommand(ticketId);
            await _mediator.Send(command, ct);
            return NoContent();
        }
        [HttpGet]
        [Route("sessions/{sessionId:guid}/tickets")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetTicketsFromSession([FromRoute] Guid sessionId, CancellationToken ct)
        {
            var result = await _mediator.Send(
            new GetTicketsFromSessionQuery(new SessionId(sessionId)), ct);
            return Ok(result);
        }
        [HttpGet]
        [Route("sessions/{sessionId:guid}/tickets/short")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetTicketsShort([FromRoute] Guid sessionId,CancellationToken ct)
        {
            var result = await _mediator.Send(new GetTicketsQuery(new SessionId(sessionId)),ct);
            return Ok(result);
        }
    }
}
