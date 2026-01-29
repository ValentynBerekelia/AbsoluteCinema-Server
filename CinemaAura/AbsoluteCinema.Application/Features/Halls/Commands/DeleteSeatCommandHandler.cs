using AbsoluteCinema.Application.Repository;
using AbsoluteCinema.Domain.Entities;
using AbsoluteCinema.Domain.Exceptions;
using MediatR;

namespace AbsoluteCinema.Application.Features.Halls.Commands;

public class DeleteSeatCommandHandler(ISeatRepository seats, IUnitOfWork unitOfWork) : IRequestHandler<DeleteSeatCommand, Unit>
{
    private readonly ISeatRepository _seats = seats;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Unit> Handle(DeleteSeatCommand request, CancellationToken cancellationToken)
    {
        SeatId seatId = new SeatId(request.SeatId);
        var seat = await _seats.GetByIdAsync(seatId, cancellationToken);

        if (seat == null)
        {
            throw new KeyNotFoundException("Seat not found");
        }
        
        await _seats.DeleteAsync(seat.Id, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        return Unit.Value;
    }
}

public record DeleteSeatCommand(
    Guid SeatId
): IRequest<Unit> {}