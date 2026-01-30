using AbsoluteCinema.Domain.Entities;

namespace AbsoluteCinema.Application.DTOs.Movie;

public record GenreDto(
    Guid Id,
    string Name
);