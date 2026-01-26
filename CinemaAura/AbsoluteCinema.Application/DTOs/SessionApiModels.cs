namespace AbsoluteCinema.Application.DTOs;

//POST
public record AdminSessionCreateRequest(
    Guid MovieId,
    string HallName,
    DateTime StartDateTime
);

// PUT
public record AdminSessionUpdatePartialRequest(
    string? HallName,
    DateTime? StartDateTime
);

// PATCH
public record AdminSessionUpdateRequest(
    Guid MovieId,
    string HallName,
    DateTime StartDateTime
);

public record SessionListItemDto(
    Guid Id,
    Guid MovieId,
    string HallName,
    DateTime StartDateTime
);