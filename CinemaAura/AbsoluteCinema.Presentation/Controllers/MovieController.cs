using System.ComponentModel.DataAnnotations;
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
using static AbsoluteCinema.Application.Features.Movies.Command.UpdateMoviePartialCommandHandler;
using AbsoluteCinema.Application.DTOs.Movie;
using AbsoluteCinema.Application.Features.Movies.Command;
using AbsoluteCinema.Application.Features.Movies.Command.CreateMovieAndAttachMedia;
using static AbsoluteCinema.Application.Features.Genres.Commands.AttachGenreToMovie.AttachGenreToMovieCommandHandler;
using AbsoluteCinema.Application.Features.Genres.Commands;

namespace AbsoluteCinema.Controllers;

[Route("api")]
[ApiController]
[ProducesResponseType(StatusCodes.Status500InternalServerError)]
public class MovieController(IMediator mediator, IMapper mapper) : ControllerBase
{
    private readonly IMediator _mediator = mediator;
    private readonly IMapper _mapper = mapper;


    [HttpPost]
    [Route("admin/movies")]
    public async Task<IActionResult> CreateMovie([FromBody] MovieCreateRequest request, CancellationToken ct)
    {
        var command = new CreateMovieCommand(
            request.MovieName,
            request.Description,
            request.Rate,
            request.AgeLimit,
            request.Duration,
            request.Country,
            request.Studio,
            request.Language,
            request.Genres
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
    public async Task<IActionResult> DeleteMovie(Guid id, CancellationToken ct)
    {
        await _mediator.Send(new DeleteMovieCommand(id), ct);
        return NoContent();
    }

    [HttpPost("admin/movies/{movieId:guid}/media/attach")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> AttachMediaToMovie(
        [FromRoute] Guid movieId,
        [FromBody] AttachMediaRequest request,
        CancellationToken ct)
    {
        var command = new AttachMediaToMovieCommand(movieId, request.mediaId);
        await _mediator.Send(command, ct);
        return NoContent();
    }

    [HttpPost("admin/movies/{movieId:guid}/media")]
    [ProducesResponseType(typeof(CreateAndAttachMediaResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> CreateAndAttachMediaToMovie(
    [FromRoute] Guid movieId,
    [FromBody] CreateMediaRequest request,
    CancellationToken ct)
    {
        var command = new CreateAndAttachMediaCommand(movieId, request.Url, request.Type);
        var response = await _mediator.Send(command, ct);
        return Ok(response);
    }

    [HttpDelete("admin/movies/{movieId:guid}/media/{mediaId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DetachMediaFromMovie(
        [FromRoute] Guid movieId,
        [FromRoute] Guid mediaId,
        CancellationToken ct)
    {
        await _mediator.Send(new DetachMediaFromMovieCommand(movieId, mediaId), ct);
        return NoContent();
    }

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

    [HttpGet("movies/features")]
    public async Task<IActionResult> GetMoviesFeatured([FromQuery] GetFeaturedMoviesQuery request, CancellationToken ct)
    {
        var response = await _mediator.Send(new GetFeaturedMoviesQuery(), ct);

        return Ok(response);
    }


    [HttpPost("admin/movies/{movieId:guid}/genre/attach")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> AttachGenreToMovie(
    [FromRoute] Guid movieId,
    [FromBody] AttachGenreRequest request,
    CancellationToken ct)
    {
        var command = new AttachGenreToMovieCommand(movieId, request.genreId);
        await _mediator.Send(command, ct);
        return NoContent();
    }

    [HttpPost("admin/movies/{movieId:guid}/genre")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> CreateGenreToMovie(
    [FromRoute] Guid movieId,
    [FromBody] CreateGenreToMovieRequest request,
    CancellationToken ct)
    {
        var command = new CreateGenreToMovieCommand(movieId,request.Name);
        await _mediator.Send(command, ct);
        return NoContent();
    }

    [HttpDelete("admin/movies/{movieId:guid}/genre/{genreId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DetachGenreFromMovie(
        [FromRoute] Guid movieId,
        [FromRoute] Guid genreId,
        CancellationToken ct)
    {
        await _mediator.Send(new DetachGenreFromMovieCommand(movieId, genreId), ct);
        return NoContent();
    }
}