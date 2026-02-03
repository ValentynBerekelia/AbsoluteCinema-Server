using AbsoluteCinema.Application.Repository;
using AbsoluteCinema.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbsoluteCinema.Application.Features.SeatTypes.Command
{
    public class DeleteSeatTypeCommandHandler(ISeatTypeRepository seatTypeRepository, IUnitOfWork unitOfWork)
        : IRequestHandler<DeleteSeatTypeCommand, Unit>
    {
        private readonly ISeatTypeRepository _seatType = seatTypeRepository;
        private readonly IUnitOfWork _unit = unitOfWork;
        public async Task<Unit> Handle(DeleteSeatTypeCommand request, CancellationToken ct)
        {
            var seatTypeId = new SeatTypeId(request.SeatTypeId);
            var seatType = await _seatType.GetByIdAsync(seatTypeId);
            if(seatType is null)
            {
                throw new KeyNotFoundException("Seat not found");
            }
            await _seatType.DeleteAsync(seatTypeId,ct);
            await _unit.SaveChangesAsync(ct);

            return Unit.Value;
        }
    }
    public record DeleteSeatTypeCommand(
        Guid SeatTypeId
        ) : IRequest<Unit>;
}
