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

namespace AbsoluteCinema.Application.Features.Movies.Command
{
    public class UpdateMovieFullCommandHandler : IRequestHandler<UpdateMovieFullCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMovieRepository _movies;
        //private readonly IGenreRepository _genres;
        //private readonly IPersonRepository _persons;

        public UpdateMovieFullCommandHandler(IUnitOfWork unitOfWork,IMovieRepository movies)
        {
            _unitOfWork = unitOfWork;
            _movies = movies;
        }
        public async Task Handle(UpdateMovieFullCommand command, CancellationToken ct)
        {
            var movie = await _movies.GetByIdForUpdateAsync(command.MovieId, ct);
            if (movie is null)
                throw new DomainException("Movie not found.");

            var r = command.Request;

            movie.ChangeName(r.Name);
            movie.ChangeDescription(r.Description);
            movie.ChangeRate(r.Rate);
            movie.ChangeAge(r.AgeLimit);
            movie.ChangeDuration(TimeSpan.FromSeconds(r.DurationSecond));
            movie.ChangeCountry(r.Country);
            movie.ChangeStudio(r.Studio);
            movie.ChangeLanguage(r.Language);

            _movies.Update(movie);
            await _unitOfWork.SaveChangesAsync(ct);
        }
    }
}
    public record UpdateMovieFullCommand(
        MovieId MovieId,
        MovieUpdateRequest Request
    ) : IRequest;
