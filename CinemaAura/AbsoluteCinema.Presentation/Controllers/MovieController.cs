using AbsoluteCinema.Application.Features.Movies.Command.DeleteMovie;
using AbsoluteCinema.Application.Features.Movies.Queries;
using AbsoluteCinema.Application.Features.Movies.Command.AttachMediaToMovie;
using AbsoluteCinema.Application.Features.Movies.Command.DetachMediaFromMovie;
using AbsoluteCinema.Domain.Entities;
using AbsoluteCinema.Requests;
using Mapster;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography.Xml;
using static AbsoluteCinema.Application.Features.Movies.Command.UpdateMoviePartialCommandHandler;

namespace AbsoluteCinema.Controllers;
[Route("api")]
public class MovieController(IMediator mediator, IMapper mapper) : ControllerBase
{
    private readonly IMediator _mediator = mediator;
    private readonly IMapper _mapper = mapper;


    [HttpPost]
    [Route("admin/movies")]
    public async Task<IActionResult> CreateMovie([FromBody]MovieCreateRequest request, CancellationToken ct)
    {
        var command = new CreateMovieCommand(
            request.MovieName,
            request.Description,
            request.Rate,
            request.AgeLimit,
            request.Duration,
            request.Country,
            request.Studio,
            request.Language
        );

        var response = await _mediator.Send(command, ct);

        return Ok(response);
    }

    [HttpGet]
    [Route("movies")]
    public async Task<IActionResult> GetMovies([FromQuery] MoviesQueryParameters filter, CancellationToken ct)
    {
        var query = filter.Adapt<GetMoviesQuery>();
        var response = await _mediator.Send(query, ct);
        return Ok(response);
    }

    [HttpGet]
    [Route("movie/{id:guid}")]
    public async Task<IActionResult> GetMovie(Guid id, CancellationToken ct)
    {
        var query = new GetMovieQuery(new MovieId(id));

        var response = await _mediator.Send(query, ct);
        return Ok(response);
    }

    [HttpDelete("admin/movies/{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteMovie(Guid id)
    {
        var command = new DeleteMovieCommand(id);
        await _mediator.Send(command);
        return NoContent();
    }

    [HttpPost("admin/movies/media")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> AttachMediaToMovie([FromBody] AttachMediaToMovieCommand command)
    {

        await _mediator.Send(command);
        return NoContent();
    }

    [HttpDelete("admin/movies/{movieId:guid}/media/{mediaId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DetachMediaFromMovie(Guid movieId, Guid mediaId)
    {
        await _mediator.Send(new DetachMediaFromMovieCommand(movieId, mediaId));

    [HttpPut("admin/movies/{movieId:guid}")]
    public async Task<IActionResult> UpdateMovieFull(
        [FromRoute] Guid movieId,
        [FromBody] MovieUpdateRequest request,
        CancellationToken ct)
    {
        await _mediator.Send(new UpdateMovieFullCommand(new MovieId(movieId), request), ct);
        return NoContent();
    }


    [HttpPatch("admin/movies/{movieId:guid}")]
    public async Task<IActionResult> UpdateMoviePartial(
    [FromRoute] Guid movieId,
    [FromBody] MovieUpdatePartialRequest request,
    CancellationToken ct)
    {
        await _mediator.Send(new UpdateMoviePartialCommand(new MovieId(movieId), request), ct);
        return NoContent();
    }
}