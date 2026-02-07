using FluentValidation;

namespace AbsoluteCinema.Application.Features.Persons.Commands.AttachMediaToPerson;

public class AttachMediaToPersonCommandValidator : AbstractValidator<AttachMediaToPersonCommand>
{
    public AttachMediaToPersonCommandValidator()
    {
        RuleFor(x => x.PersonId)
            .NotEmpty()
            .WithMessage("Person ID is required.");

        RuleFor(x => x.Url)
                .NotEmpty()
                .WithMessage("URL is required.")
                .MaximumLength(2000)
                .WithMessage("URL cannot exceed 2000 characters.")
                .Must(BeAValidUrl)
                .WithMessage("URL must be a valid HTTP or HTTPS URL.");

    }

    private static bool BeAValidUrl(string url)
    {
        if (string.IsNullOrWhiteSpace(url))
            return false;

        return Uri.TryCreate(url, UriKind.Absolute, out var uriResult)
               && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
    }
}