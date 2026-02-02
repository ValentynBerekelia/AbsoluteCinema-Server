using AbsoluteCinema.Application.Features.Genres.Commands;
using AbsoluteCinema.Application.Features.Genres.Queries;
using AbsoluteCinema.Domain.Entities;
using AbsoluteCinema.Requests;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AbsoluteCinema.Controllers
{
    [Route("api")]
    [ApiController]
    public class GenreController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;
        [HttpGet]
        [Route("genres")]
        public async Task<IActionResult> GetGenres([FromQuery] Guid? movieId, CancellationToken ct)
        {
            var response = await _mediator.Send(new GetGenresQuery(movieId), ct);
            return Ok(response);
        }

        [HttpPost]
        [Route("/genres")]
        public async Task<IActionResult> CreateGenre([FromBody] CreateGenreRequest request, CancellationToken ct)
        {
            var result = await _mediator.Send(new CreateGenreCommand(request.GenreName),ct);
            return CreatedAtAction(nameof(GetGenres), new { id = result.GenreId });
        }

        [HttpPut]
        [Route("genre/{genreId:guid}")]
        public async Task<IActionResult> UpdateGenre([FromRoute] Guid genreId, [FromBody]UpdateGenreRequest command  ,CancellationToken ct)
        {
            await _mediator.Send(new UpdateFullGenreCommand(genreId, command.Name),ct);
            return NoContent();
        }

        [HttpDelete]
        [Route("genre/{genreId:guid}")]
        public async Task<IActionResult> DeleteGenre([FromRoute] Guid genreId, CancellationToken ct)
        {
            var command = new DeleteGenreCommand(genreId);
            await _mediator.Send(command, ct);
            return NoContent();
        }
    }
}
