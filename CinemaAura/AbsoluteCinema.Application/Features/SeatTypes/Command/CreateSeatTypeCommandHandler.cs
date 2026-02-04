using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AbsoluteCinema.Application.Repository;
using AbsoluteCinema.Domain.Entities;
using MediatR;
using static AbsoluteCinema.Application.Features.SeatTypes.Command.CreateSeatTypeCommandHandler;

namespace AbsoluteCinema.Application.Features.SeatTypes.Command
{
    public class CreateSeatTypeCommandHandler(ISeatTypeRepository seatTypeRepository, IUnitOfWork unitOfWork)
        : IRequestHandler<CreateSeatTypeCommand, CreateSeatTypeResponse>
    {
        private readonly ISeatTypeRepository _seatType = seatTypeRepository;
        private readonly IUnitOfWork _unit = unitOfWork;

        public async Task<CreateSeatTypeResponse> Handle(CreateSeatTypeCommand command, CancellationToken ct)
        {
            var seatType = SeatType.Create(command.Name);

            await _seatType.AddAsync(seatType, ct);
            await _unit.SaveChangesAsync(ct);

            return new CreateSeatTypeResponse(seatType.Id, seatType.TypeName);
        }
        public record CreateSeatTypeRequest(
            string Name);
        public record CreateSeatTypeCommand(
            string Name
        ) : IRequest<CreateSeatTypeResponse>;

        public record CreateSeatTypeResponse(
            SeatTypeId SeatTypeId,
            string Name
        );
    }
}
