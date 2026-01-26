using FluentValidation;

namespace AbsoluteCinema.Application.Features.Movies.Command.AttachMediaToMovie;

public class AttachMediaToMovieCommandValidator : AbstractValidator<AttachMediaToMovieCommand>
{
    public AttachMediaToMovieCommandValidator()
    {
        RuleFor(x => x.MovieId)
            .NotEmpty().WithMessage("Movie ID is required.");

        RuleFor(x => x.MediaId)
            .NotEmpty().WithMessage("Media ID is required.");
    }
}