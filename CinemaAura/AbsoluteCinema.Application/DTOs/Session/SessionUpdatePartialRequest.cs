using AbsoluteCinema.Domain.Entities;

namespace AbsoluteCinema.Application.DTOs;

public record SessionUpdatePartialRequest(
    MovieId? MovieID,
    HallId? HallId,
    MovieFormat? Format,
    DateTime? StartDateTime,
    Dictionary<Guid, decimal>? SeatPrices
    
);