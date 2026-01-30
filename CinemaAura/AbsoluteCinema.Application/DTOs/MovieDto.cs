using System.Collections;
using AbsoluteCinema.Domain.Entities;

namespace AbsoluteCinema.Application.DTOs;

public record MovieDto
(
     MovieId Id,
     string Name,
     string? PosterUrl,
     decimal Rate,
     int AgeLimit,
     TimeSpan Duration,
     IEnumerable<string> Genres,
     IEnumerable<SessionDto> Sessions
         ){}