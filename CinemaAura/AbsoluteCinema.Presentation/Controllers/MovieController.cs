using AbsoluteCinema.Application.DTOs.Movie;
using AbsoluteCinema.Application.Features.Movies.Command;
using AbsoluteCinema.Domain.Entities;
using AbsoluteCinema.Application.Features.Movies.Queries;
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
    [HttpPut]
    [Route("/admin/movie/{id:guid}")]
    public async Task<IActionResult> UpdateMovieFull(Guid movieId, [FromBody] MovieUpdateRequest request, CancellationToken ct)
    {
        await _mediator.Send(new UpdateMovieFullCommand(new MovieId(movieId), request), ct);
        return NoContent();
    }
    [HttpPatch]
    [Route("/admin/movie/{id:guid}")]
    public async Task<IActionResult> UpdateMoviePartial(Guid movieId, [FromBody] MovieUpdatePartialRequest request,CancellationToken ct)
    {
        await _mediator.Send(new UpdateMoviePartialCommand(new MovieId(movieId), request), ct);
        return NoContent();
    }
}