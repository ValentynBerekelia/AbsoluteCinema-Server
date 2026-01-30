using AbsoluteCinema.Application.Repository;
using AbsoluteCinema.Domain.Entities;
using AbsoluteCinema.Domain.Exceptions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbsoluteCinema.Application.Features.Halls.Commands
{
    public class DeleteHallsCommanHandler : IRequestHandler<DeleteHallCommand, Unit>
    {
        private readonly IHallRepository _hall;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteHallsCommanHandler(IHallRepository hallRepository, IUnitOfWork unitOfWork)
        {
            _hall = hallRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(DeleteHallCommand command,CancellationToken ct)
        {
            var hallId = new HallId(command.HallId);
            var exist = await _hall.AnyAsync(hallId, ct);
            if (!exist)
            {
                throw new DomainException($"Hall with ID {command.HallId} not found");
            }
            await _hall.DeleteAsync(hallId, ct);
            await _unitOfWork.SaveChangesAsync(ct);

            return Unit.Value;
        }
    }
    public record DeleteHallCommand(Guid HallId) : IRequest<Unit>;
}
