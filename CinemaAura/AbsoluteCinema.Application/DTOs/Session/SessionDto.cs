using AbsoluteCinema.Application.DTOs.Hall;
using AbsoluteCinema.Domain.Entities;

namespace AbsoluteCinema.Application.DTOs;

public record SessionDto(
    Guid Id,
    DateTime startDateTime,
    MovieFormat? Format
)
{}
