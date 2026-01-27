using FluentValidation;

namespace AbsoluteCinema.Application.Features.Movies.Command.DetachMediaFromMovie
{
    public class DetachMediaFromMovieCommandValidator : AbstractValidator<DetachMediaFromMovieCommand>
    {
        public DetachMediaFromMovieCommandValidator()
        {
            RuleFor(x => x.MovieId)
                .NotEmpty().WithMessage("Movie ID is required.");

            RuleFor(x => x.MediaId)
                .NotEmpty().WithMessage("Media ID is required.");
        }
    }
}
