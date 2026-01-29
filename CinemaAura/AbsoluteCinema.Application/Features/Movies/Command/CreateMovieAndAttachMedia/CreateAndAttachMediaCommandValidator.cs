using AbsoluteCinema.Domain.Enums;
using FluentValidation;

namespace AbsoluteCinema.Application.Features.Movies.Command.CreateMovieAndAttachMedia
{
    public class CreateAndAttachMediaCommandValidator : AbstractValidator<CreateAndAttachMediaCommand>
    {
        public CreateAndAttachMediaCommandValidator()
        {
            RuleFor(x => x.MovieId)
                .NotEmpty()
                .WithMessage("Movie ID is required.");

            RuleFor(x => x.Url)
                .NotEmpty()
                .WithMessage("URL is required.")
                .MaximumLength(2000)
                .WithMessage("URL cannot exceed 2000 characters.")
                .Must(BeAValidUrl)
                .WithMessage("URL must be a valid HTTP or HTTPS URL.");

            RuleFor(x => x.Type)
                .IsInEnum()
                .WithMessage($"Invalid media type. Valid values: {string.Join(", ", Enum.GetNames<MediaType>())}.");
        }

        private static bool BeAValidUrl(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
                return false;

            return Uri.TryCreate(url, UriKind.Absolute, out var uriResult)
                   && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
        }
    }
}