using AbsoluteCinema.Domain.Entities;

namespace AbsoluteCinema.Application.DTOs;

public record SeatTypeDto(
    SeatTypeId SeatTypeId,
    string Name
){}