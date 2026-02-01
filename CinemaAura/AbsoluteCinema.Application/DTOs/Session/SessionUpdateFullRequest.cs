using AbsoluteCinema.Domain.Entities;

namespace AbsoluteCinema.Application.DTOs;

public record SessionUpdateFullRequest(
    MovieId MovieId,
    HallId HallId,
    MovieFormat Format,
    DateTime StartDateTime,
    Dictionary<Guid, decimal> SeatPrices
);