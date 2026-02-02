namespace AbsoluteCinema.Requests
{
    public record UpdateTicketRequest(
        Guid SessionId,
        Guid SeatId,
        Guid UserId
        );
}
