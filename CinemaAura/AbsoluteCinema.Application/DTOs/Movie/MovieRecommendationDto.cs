namespace AbsoluteCinema.Application.DTOs;

public record MovieRecommendationDto (
    Guid Id,
    string Name,
    string PosterUrl
);