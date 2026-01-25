using AbsoluteCinema.Domain.Entities;

namespace AbsoluteCinema.Application.DTOs;

public record SessionUpdatePartialRequest(
    Guid? MovieID,
    Guid? HallId,
    MovieFormat? Format,
    DateTime? StartDateTime
);