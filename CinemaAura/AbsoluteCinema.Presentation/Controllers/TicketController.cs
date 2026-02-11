using AbsoluteCinema.Application.Features.Halls.Queries;
using AbsoluteCinema.Application.Features.Sessions.Queries;
using AbsoluteCinema.Application.Features.Tickets.Commands;
using AbsoluteCinema.Application.Features.Tickets.Queries;
using AbsoluteCinema.Domain.Entities;
using AbsoluteCinema.Infrastructure.EFQueries;
using AbsoluteCinema.Infrastructure.Security;
using AbsoluteCinema.Requests;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static AbsoluteCinema.Application.Features.Tickets.Commands.CreateTicketCommandHandler;
namespace AbsoluteCinema.Controllers
{
    [ApiController]
    [Route("api")]
    public class TicketController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;
        [Authorize]
        [HttpGet]
        [Route("ticket/{ticketId:guid}")]
        public async Task<IActionResult> GetTicket(Guid ticketId, CancellationToken ct)
        {
            var query = new GetTicketQuery(new TicketId(ticketId));
            var response = await _mediator.Send(query, ct);
            return Ok(response);
        }

        [Authorize(Policy = Permissions.TicketsCreate)]
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
        [Authorize(Policy = Permissions.TicketsManage)]
        [HttpPatch]
        [Route("ticket/{ticketId:guid}")]
        public async Task<IActionResult> UpdateTicket([FromRoute] Guid ticketId, [FromBody] UpdateTicketRequest request, CancellationToken ct)
        {
            var command = new UpdateTicketCommand(ticketId, request.SessionId, request.SeatId, request.UserId);
            await _mediator.Send(command, ct);
            return NoContent();
        }
        [Authorize(Policy = Permissions.TicketsManage)]
        [HttpDelete]
        [Route("ticket/{ticketId:guid}")]
        public async Task<IActionResult> DeleteTicket([FromRoute] Guid ticketId, CancellationToken ct)
        {
            var command = new DeleteTicketCommand(ticketId);
            await _mediator.Send(command, ct);
            return NoContent();
        }
        [AllowAnonymous]
        [HttpGet]
        [Route("sessions/{sessionId:guid}/tickets")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetTicketsFromSession([FromRoute] Guid sessionId, CancellationToken ct)
        {
            var result = await _mediator.Send(
            new GetTicketsFromSessionQuery(new SessionId(sessionId)), ct);
            return Ok(result);
        }
        [AllowAnonymous]
        [HttpGet]
        [Route("sessions/{sessionId:guid}/tickets/short")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetTicketsShort([FromRoute] Guid sessionId,CancellationToken ct)
        {
            var result = await _mediator.Send(new GetTicketsQuery(new SessionId(sessionId)),ct);
            return Ok(result);
        }

        [Authorize]
        [HttpPost]
        [Route("ticket/{ticketId:guid}/confirm")]
        public async Task<IActionResult> ConfirmTicket([FromRoute] Guid ticketId)
        {
            await _mediator.Send(new ConfirmTicketCommand(ticketId));
            return Ok(new { message = "Payment successful! Ticket confirmed." });
        }

        [Authorize]
        [HttpDelete]
        [Route("ticket/{ticketId:guid}/cancel")]
        public async Task<IActionResult> CancelTicket([FromRoute] Guid ticketId)
        {
            var userIdString = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            if (!Guid.TryParse(userIdString, out var userId))
            {
                return Unauthorized();
            }

            try
            {
                await _mediator.Send(new CancelTicketCommand(ticketId, userId));
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}
