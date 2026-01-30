using AbsoluteCinema.Domain.Entities;

namespace AbsoluteCinema.Application.DTOs;

public record SessionDto(
    SessionId Id,
    DateTime startDateTime,
    MovieFormat? Format
){}