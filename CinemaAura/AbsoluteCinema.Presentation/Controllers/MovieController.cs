using AbsoluteCinema.Application.DTOs.Movie;
using AbsoluteCinema.Application.Features.Movies.Command;
using AbsoluteCinema.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography.Xml;
using static AbsoluteCinema.Application.Features.Movies.Command.UpdateMoviePartialCommandHandler;

namespace AbsoluteCinema.Controllers;
[Route("api")]
public class MovieController : ControllerBase
{
    private readonly IMediator _mediator;
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
