using AbsoluteCinema.Application.Features.Halls.Commands;
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
    public class UpdateSeatTypeCommandHandler(ISeatTypeRepository seatTypeRepository, IUnitOfWork unitOfWork)
        : IRequestHandler<UpdateSeatTypeCommand, Unit>
    {
        private readonly ISeatTypeRepository _seatType = seatTypeRepository;
        private readonly IUnitOfWork _unit = unitOfWork;

        public async Task<Unit> Handle(UpdateSeatTypeCommand command, CancellationToken ct)
        {
            var seatTypeId = new SeatTypeId(command.SeatTypeId);
            var seatType = await _seatType.GetByIdForUpdateAsync(seatTypeId, ct);
            if(seatType is null)
            {
                new KeyNotFoundException($"SeatType Id {command.SeatTypeId} not found");
            }
            if (!string.IsNullOrWhiteSpace(command.Name))
            {
                seatType.ChangeTypeName(command.Name);
            }
            _seatType.Update(seatType);

            await _unit.SaveChangesAsync(ct);
            return Unit.Value;
        }
    }
    public record UpdateSeatTypeCommand(
        Guid SeatTypeId,
        string Name
        ) : IRequest<Unit>;
}
