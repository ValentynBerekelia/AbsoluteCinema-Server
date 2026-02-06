using AbsoluteCinema.Domain.Enums;
using FluentValidation;

namespace AbsoluteCinema.Application.Features.Persons.Commands.AttachPersonToMovie
{
    public class AttachPersonToMovieCommandValidator : AbstractValidator<AttachPersonToMovieCommand>
    {
        public AttachPersonToMovieCommandValidator()
        {
            RuleFor(x => x.MovieId)
                .NotEmpty()
                .WithMessage("Movie ID is required.");

            RuleFor(x => x.PersonId)
                .NotEmpty()
                .WithMessage("Person ID is required.");

            RuleFor(x => x.Role)
                .IsInEnum()
                .WithMessage(_ =>
                {
                    var validRoles = string.Join(", ",
                        Enum.GetValues(typeof(PersonRole))
                            .Cast<PersonRole>()
                            .Select(r => $"{r} ({(int)r})"));

                    return $"Invalid person role. Valid values: {validRoles}.";
                });

        }
    }
}
