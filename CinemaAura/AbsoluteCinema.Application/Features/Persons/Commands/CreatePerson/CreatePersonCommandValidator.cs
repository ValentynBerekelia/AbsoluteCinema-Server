using AbsoluteCinema.Domain.Enums;
using FluentValidation;

namespace AbsoluteCinema.Application.Features.Persons.Commands.CreatePerson;

public class CreatePersonCommandValidator : AbstractValidator<CreatePersonCommand>
{
    public CreatePersonCommandValidator()
    {
        RuleFor(x => x.FullName)
            .NotEmpty()
            .WithMessage("Full name is required.")
            .MinimumLength(1)
            .WithMessage("Full name must be at least 1 characters long.")
            .MaximumLength(100)
            .WithMessage("Full name cannot exceed 100 characters.")
            .Matches(@"^[\p{L}\s\-'\.]+$")
            .WithMessage("Full name can only contain letters, spaces, hyphens, apostrophes and dots.");

        RuleFor(x => x.Bio)
            .MaximumLength(2000)
            .WithMessage("Bio cannot exceed 2000 characters.")
            .When(x => !string.IsNullOrEmpty(x.Bio));

        RuleFor(x => x.BirthDate)
            .NotEmpty()
            .WithMessage("Birth date is required.")
            .LessThan(DateTime.UtcNow.Date)
            .WithMessage("Birth date cannot be in the future.")
            .GreaterThan(DateTime.UtcNow.AddYears(-200))
            .WithMessage("Birth date seems unrealistic (more than 200 years ago).");

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