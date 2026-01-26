using AbsoluteCinema.Application.DTOs.Movie;
using AbsoluteCinema.Application.Repository;
using AbsoluteCinema.Domain.Entities;
using AbsoluteCinema.Domain.Exceptions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static AbsoluteCinema.Application.Features.Movies.Command.UpdateMoviePartialCommandHandler;

namespace AbsoluteCinema.Application.Features.Movies.Command
{
    public class UpdateMoviePartialCommandHandler : IRequestHandler<UpdateMoviePartialCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMovieRepository _movies;
        public async Task Handle(UpdateMoviePartialCommand command, CancellationToken ct)
        {
            var movie = await _movies.GetByIdForUpdateAsync(command.MovieId, ct);
            if (movie is null)
                throw new DomainException("Movie not found.");

            var r = command.Request;

            if (r.Name is null &&
                r.Description is null &&
                r.Rate is null &&
                r.AgeLimit is null &&
                r.DurationSeconds is null &&
                r.Country is null &&
                r.Studio is null &&
                r.Language is null)

            if (r.Name is not null)
                movie.ChangeName(r.Name);

            if (r.Description is not null)
                movie.ChangeDescription(r.Description);

            if (r.Rate.HasValue)
                movie.ChangeRate(r.Rate.Value);

            if (r.AgeLimit.HasValue)
                movie.ChangeAge(r.AgeLimit.Value);

            if (r.DurationSeconds.HasValue)
                movie.ChangeDuration(TimeSpan.FromSeconds(r.DurationSeconds.Value));

            if (r.Country is not null)
                movie.ChangeCountry(r.Country);

            if (r.Studio is not null)
                movie.ChangeStudio(r.Studio);

            if (r.Language is not null)
                movie.ChangeLanguage(r.Language);

            _movies.Update(movie);
            await _unitOfWork.SaveChangesAsync(ct);
        }
        public record UpdateMoviePartialCommand(MovieId MovieId, MovieUpdatePartialRequest Request) : IRequest;
    }
}
