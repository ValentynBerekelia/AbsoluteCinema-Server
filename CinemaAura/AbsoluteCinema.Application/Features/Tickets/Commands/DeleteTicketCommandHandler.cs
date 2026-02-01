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
    public class DeleteTicketCommandHandler(ITicketRepository ticketRepository,IUnitOfWork unitOfWork)
    : IRequestHandler<DeleteTicketCommand, Unit>
    {
        private readonly ITicketRepository _ticket = ticketRepository;
        private readonly IUnitOfWork _unit = unitOfWork;

        public async Task<Unit> Handle(DeleteTicketCommand request, CancellationToken ct)
        {
            TicketId ticketId = new TicketId(request.TicketId);
            var ticket = await _ticket.GetByIdForUpdateAsync(ticketId, ct);

            await _ticket.DeleteAsync(ticket.Id, ct);
            await _unit.SaveChangesAsync(ct);

            return Unit.Value;
        }

    }

    public record DeleteTicketCommand(
        Guid TicketId
        ) :IRequest<Unit>;
}
