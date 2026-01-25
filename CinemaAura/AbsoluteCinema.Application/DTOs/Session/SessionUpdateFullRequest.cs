using AbsoluteCinema.Domain.Entities;

namespace AbsoluteCinema.Application.DTOs;

public record SessionUpdateFullRequest(
    Guid MovieId,
    Guid HallId,
    MovieFormat Format,
    DateTime StartDateTime
);