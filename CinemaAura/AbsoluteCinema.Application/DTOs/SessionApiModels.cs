namespace AbsoluteCinema.Application.DTOs;

//POST
public record AdminSessionCreateRequest(
    Guid MovieId,
    Guid HallId,
    DateTime StartDateTime
);

// PUT
public record AdminSessionUpdatePartialRequest(
    DateTime? StartDateTime
);

// PATCH
public record AdminSessionUpdateRequest(
    Guid MovieId,
    Guid HallId,
    DateTime StartDateTime
);

public record SessionListItemDto(
    Guid Id,
    Guid MovieId,
    Guid HallId,
    DateTime StartDateTime
);