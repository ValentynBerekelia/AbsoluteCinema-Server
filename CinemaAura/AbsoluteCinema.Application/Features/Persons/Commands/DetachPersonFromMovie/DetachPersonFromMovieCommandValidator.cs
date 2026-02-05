using FluentValidation;

namespace AbsoluteCinema.Application.Features.Persons.Commands.DetachPersonFromMovie;

public class DetachPersonFromMovieCommandValidator : AbstractValidator<DetachPersonFromMovieCommand>
{
    public DetachPersonFromMovieCommandValidator()
    {
        RuleFor(x => x.MovieId)
            .NotEmpty()
            .WithMessage("Movie ID is required.");

        RuleFor(x => x.PersonId)
            .NotEmpty()
            .WithMessage("Person ID is required.");
    }
}