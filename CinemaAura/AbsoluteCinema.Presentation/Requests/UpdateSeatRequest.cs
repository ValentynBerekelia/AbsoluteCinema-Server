using MediatR;

namespace AbsoluteCinema.Requests
{
    public record UpdateSeatRequest(
        int Row,
        int Number,
        Guid SeatTypeId
        );
}
