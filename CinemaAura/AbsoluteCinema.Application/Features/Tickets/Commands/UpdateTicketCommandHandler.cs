using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AbsoluteCinema.Application.Repository;
using AbsoluteCinema.Domain.Entities;
using MediatR;

namespace AbsoluteCinema.Application.Features.Tickets.Commands
{
    public class UpdateTicketCommandHandler(ITicketRepository ticketRepository,IUnitOfWork unitOfWork,ISessionRepository sessionRepository,IUserRepository userRepository,ISeatRepository seatRepository)
    : IRequestHandler<UpdateTicketCommand,Unit>
    {
        private readonly ITicketRepository _ticket = ticketRepository;
        private readonly IUserRepository _user = userRepository;
        private ISessionRepository _session = sessionRepository;
        private ISeatRepository _seat = seatRepository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        public async Task<Unit> Handle(UpdateTicketCommand request , CancellationToken ct)
        {
            var ticketId = new TicketId(request.TicketId);
            var ticket = await _ticket.GetByIdForUpdateAsync(ticketId, ct);
            if (ticket is null)
            {
                throw new KeyNotFoundException($"Seat with id {request.SeatId} not found");
            }

            if (request.SessionId.HasValue)
            {
                ticket.ChangeSession(new SessionId(request.SessionId.Value));
            }

            if (request.SeatId.HasValue)
            {
                ticket.ChangeSeat(new SeatId(request.SeatId.Value));
            }

            if (request.UserId.HasValue)
            {
                ticket.ChangeUser(new UserId(request.UserId.Value));
            }

            _ticket.Update(ticket);
            await unitOfWork.SaveChangesAsync(ct);
            return Unit.Value;
        }
    }

    public record UpdateTicketCommand(
        Guid TicketId,
        Guid? SessionId,
        Guid? SeatId,
        Guid? UserId
        ) :IRequest<Unit>;
}
