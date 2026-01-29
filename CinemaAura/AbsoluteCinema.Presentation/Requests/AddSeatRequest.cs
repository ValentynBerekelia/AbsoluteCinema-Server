namespace AbsoluteCinema.Requests;

public record AddSeatRequest(
    Guid HallId,
    Guid SeatTypeId,
    IReadOnlyCollection<SeatInputDto> Seats);

public record SeatInputDto(
    int Row,
    int Number
);
