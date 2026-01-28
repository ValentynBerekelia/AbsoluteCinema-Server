using AbsoluteCinema.Domain.Entities;

namespace AbsoluteCinema.Application.DTOs;

//POST
public record AdminSessionCreateRequest(
    Guid MovieId,
    Guid HallId,
    DateTime StartDateTime,
    MovieFormat Format
);

public record SessionListItemDto(
    Guid Id,
    Guid MovieId,
    Guid HallId,
    DateTime StartDateTime,
    MovieFormat Format
);