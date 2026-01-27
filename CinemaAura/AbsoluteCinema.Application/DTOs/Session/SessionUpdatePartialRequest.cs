using AbsoluteCinema.Domain.Entities;

namespace AbsoluteCinema.Application.DTOs;

public record SessionUpdatePartialRequest(
    MovieId? MovieID,
    MovieId? HallId,
    MovieFormat? Format,
    DateTime? StartDateTime
);