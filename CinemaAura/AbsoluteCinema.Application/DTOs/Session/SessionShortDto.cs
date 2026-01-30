using AbsoluteCinema.Domain.Entities;

namespace AbsoluteCinema.Application.DTOs;

public record SessionShortDto(
    SessionId SessionId,
    DateTime StartDateTime
){}