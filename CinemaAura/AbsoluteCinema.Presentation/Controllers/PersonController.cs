using AbsoluteCinema.Application.Features.Persons.Commands.AttachMediaToPerson;
using AbsoluteCinema.Application.Features.Persons.Commands.CreatePerson;
using AbsoluteCinema.Application.Features.Persons.Commands.DeletePerson;
using AbsoluteCinema.Application.Features.Persons.Commands.UpdatePerson;
using AbsoluteCinema.Application.Features.Persons.Queries;
using AbsoluteCinema.Requests;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AbsoluteCinema.Controllers;

[Route("api")]
[ApiController]
[ProducesResponseType(StatusCodes.Status500InternalServerError)]
public class PersonController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    private const int MaxLimit = 80;
    private const int DefaultLimit = 20;

    /// <summary>
    /// Search persons (for autocomplete/dropdown at FE)
    /// </summary>
    [HttpGet("persons")]
    [ProducesResponseType(typeof(IEnumerable<PersonListItem>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetPersons(
        [FromQuery] GetPersonsQueryParameters parameters,
        CancellationToken ct)
    {

        var requestedLimit = parameters.Limit <= 0 ? DefaultLimit : parameters.Limit;
        var limit = Math.Min(requestedLimit, MaxLimit);

        var query = new GetPersonsQuery(parameters.Search, parameters.Role, limit);
        var response = await _mediator.Send(query, ct);
        return Ok(response);
    }

    [HttpGet("persons/{id:guid}")]
    [ProducesResponseType(typeof(GetPersonResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetPerson(Guid id, CancellationToken ct)
    {
        var response = await _mediator.Send(new GetPersonQuery(id), ct);
        return Ok(response);
    }

    [HttpPost("admin/persons")]
    [ProducesResponseType(typeof(CreatePersonResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreatePerson(
        [FromBody] CreatePersonRequest request,
        CancellationToken ct)
    {
        var command = new CreatePersonCommand(
            request.FullName,
            request.Bio,
            request.BirthDate,
            request.Role
        );
        var response = await _mediator.Send(command, ct);
        return Ok(response);
    }

    [HttpPut("admin/persons/{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdatePerson(
        [FromRoute] Guid id,
        [FromBody] UpdatePersonRequest request,
        CancellationToken ct)
    {
        var command = new UpdatePersonCommand(
            id,
            request.FullName,
            request.Bio,
            request.BirthDate,
            request.Role
        );
        await _mediator.Send(command, ct);
        return NoContent();
    }

    [HttpDelete("admin/persons/{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeletePerson(Guid id, CancellationToken ct)
    {
        await _mediator.Send(new DeletePersonCommand(id), ct);
        return NoContent();
    }

    [HttpPost("admin/persons/{personId:guid}/media")]
    [ProducesResponseType(typeof(AttachMediaToPersonResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> AttachMediaToPerson(
        [FromRoute] Guid personId,
        [FromBody] CreatePersonMediaRequest request,
        CancellationToken ct)
    {
        var command = new AttachMediaToPersonCommand(personId, request.Url);
        var response = await _mediator.Send(command, ct);
        return Ok(response);
    }
}
