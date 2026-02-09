using AbsoluteCinema.Domain.Enums;
using FluentValidation;

namespace AbsoluteCinema.Application.Features.Movies.Command.CreateMovieAndAttachMedia
{
    public class CreateAndAttachMediaCommandValidator : AbstractValidator<CreateAndAttachMediaCommand>
    {
        private readonly string[] _allowedImageExtensions = { ".jpg", ".jpeg", ".png", ".webp", ".svg" };
        public CreateAndAttachMediaCommandValidator()
        {
            RuleFor(x => x.MovieId)
                .NotEmpty()
                .WithMessage("Movie ID is required.");

            RuleFor(x => x.Type)
                .IsInEnum()
                .WithMessage($"Invalid media type. Valid values: {string.Join(", ", Enum.GetNames<MediaType>())}.");

            RuleFor(x => x)
                .Must(x => x.FileStream != null || !string.IsNullOrWhiteSpace(x.ExternalUrl))
                .WithMessage("Either a file or an external URL must be provided");

            RuleFor(x => x.ExternalUrl)
                .MaximumLength(2000)
                .Must(BeAValidUrl)
                .When(x => !string.IsNullOrWhiteSpace(x.ExternalUrl))
                .WithMessage("URL must be a valid HTTP or HTTPS URL.");
            
            RuleFor(x => x.ExternalUrl)
                .NotEmpty()
                .When(x => x.Type == MediaType.Video)
                .WithMessage("Trailers must be provided as an external link.");

            When(x => x.FileStream != null, () =>
            {
                RuleFor(x => x.FileName)
                    .NotEmpty()
                    .WithMessage("File name is required when uploading a file.")
                    .Must(BeAnAllowedExtension)
                    .WithMessage($"Invalid file type. Allowed: {string.Join(", ", _allowedImageExtensions)}");

                RuleFor(x => x.FileStream!.Length)
                    .GreaterThan(0)
                    .WithMessage("File cannot be empty.");

                RuleFor(x => x.FileStream!.Length)
                    .LessThanOrEqualTo(10 * 1024 * 1024)
                    .WithMessage("File size cannot exceed 10 MB.");
            });

        }

        private bool BeAnAllowedExtension(string? fileName)
        {
            if (string.IsNullOrEmpty(fileName)) return false;
            var extension = Path.GetExtension(fileName).ToLower();
            return _allowedImageExtensions.Contains(extension);
        }
        private static bool BeAValidUrl(string? url)
        {
            if (string.IsNullOrWhiteSpace(url))
                return false;

            return Uri.TryCreate(url, UriKind.Absolute, out var uriResult)
                   && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
        }
    }
}