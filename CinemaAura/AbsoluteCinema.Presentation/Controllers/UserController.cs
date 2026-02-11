using AbsoluteCinema.Application.Features.Users.Queries;
using AbsoluteCinema.Infrastructure.Security;
using AbsoluteCinema.Requests;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AbsoluteCinema.Presentation;

[Route("api")]
[ApiController]
[ProducesResponseType(StatusCodes.Status500InternalServerError)]
public class UserController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [Authorize(Policy = Permissions.UsersManage)]
    [HttpGet]
    [Route("admin/users")]
    public async Task<IActionResult> GetUsersWithTicketsCount([FromQuery] UsersWithTicketsCountParameters filter, CancellationToken ct)
    {
        var query = new GetUsersWithTicketsCountQuery(filter.PageNumber, filter.PageSize, filter.SearchTerm);

        var response = await _mediator.Send(query, ct);
        return Ok(response);
    }
}