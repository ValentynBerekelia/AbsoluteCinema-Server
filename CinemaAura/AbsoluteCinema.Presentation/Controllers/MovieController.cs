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

    [HttpGet]
    [Route("movies")]
    public async Task<IActionResult> GetMovies([FromQuery] MoviesQueryParameters filter)
    {
        var query = filter.Adapt<GetMoviesQuery>();
        var response = await _mediator.Send(query);
        return Ok(response);
    }
    
    [HttpGet]
    [Route("movie/{id:guid}")]
    public async Task<IActionResult> GetMovie(Guid id)
    {
        var query = new GetMovieQuery(new MovieId(id));
    
        var response = await _mediator.Send(query);
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
    public async Task<IActionResult> UpdateMoviePartial(Guid muvieId, [FromBody] MovieUpdatePartialRequest request,CancellationToken ct)
    {
        await _mediator.Send(new UpdateMoviePartialCommand(new MovieId(muvieId), request), ct);
        return NoContent();
    }
}