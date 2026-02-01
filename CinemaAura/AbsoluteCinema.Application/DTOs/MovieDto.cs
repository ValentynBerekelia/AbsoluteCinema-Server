using System.Collections;
using AbsoluteCinema.Application.DTOs.Movie;
using AbsoluteCinema.Domain.Entities;

namespace AbsoluteCinema.Application.DTOs;

public record MovieDto
(
     Guid Id,
     string Name,
     string? PosterUrl,
     decimal Rate,
     int AgeLimit,
     TimeSpan Duration,
     IEnumerable<GenreDto> Genres,
     IEnumerable<SessionDto> Sessions
         ){}