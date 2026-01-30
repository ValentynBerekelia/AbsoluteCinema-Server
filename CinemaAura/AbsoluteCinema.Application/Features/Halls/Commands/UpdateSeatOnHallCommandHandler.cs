using AbsoluteCinema.Application.Repository;
using AbsoluteCinema.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbsoluteCinema.Application.Features.Halls.Commands
{
    public class UpdateSeatOnHallCommandHandler(IHallRepository halls, ISeatRepository seat, IUnitOfWork unitOfWork)
        : IRequestHandler<UpdateSeatCommand,Unit>
    {
        private readonly IHallRepository _halls = halls;
        private readonly ISeatRepository _seat = seat;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        public async Task<Unit> Handle(UpdateSeatCommand request, CancellationToken ct)
        {
            var hallId = new HallId(request.HallId);
            var seatId = new SeatId(request.SeatId);
            var seat = await _seat.GetByIdForUpdateAsync(seatId, ct);
            var halls = await _halls.GetByIdForUpdateAsync(hallId, ct);

            if (halls is null)
            {
                throw new KeyNotFoundException($"Hall with id {request.HallId} not found");
            }
            if(seat is null)
            {
                throw new KeyNotFoundException($"Seat with id {request.SeatId} not found");
            }
            if(seat.HallId != hallId)
            {
                throw new KeyNotFoundException($"Seat {request.SeatId} does not belong to hall {request.HallId}");
            }
            seat.ChangeRow((short)request.Row);
            seat.ChangeNumber((short)request.Number);
            seat.ChangeSeatTypeId(new SeatTypeId(request.SeatTypeId));

            _seat.Update(seat);
            await _unitOfWork.SaveChangesAsync(ct);
            return Unit.Value;
        }
    }
    public record UpdateSeatCommand(
        Guid HallId,
        Guid SeatId,
        int Row,
        int Number,
        Guid SeatTypeId
        ) : IRequest<Unit>;
    public record UpdateSeatResponse(
        int Row,
        int Number,
        Guid SeatTypeId
        );
}
