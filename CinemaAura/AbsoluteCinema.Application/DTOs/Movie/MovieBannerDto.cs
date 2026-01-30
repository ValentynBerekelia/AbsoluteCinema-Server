using AbsoluteCinema.Domain.Entities;

namespace AbsoluteCinema.Application.DTOs;

public record MovieBannerDto
(
    MovieId MovieId,
    string Name,
    string? BannerUrl,
    IEnumerable<SessionShortDto> TodaySessions
){}