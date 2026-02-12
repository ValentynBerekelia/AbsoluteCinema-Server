using AbsoluteCinema.Application.Repository;
using AbsoluteCinema.Domain.Entities;
using MediatR;

namespace AbsoluteCinema.Application.Features.Tickets.Commands;

public record UpdateTicketStatusCommand(
    TicketId Id,
    TicketStatus NewStatus,
    UserId UserId,
    bool IsAdmin
) : IRequest<Guid>;

public class UpdateTicketStatusCommandHandler(
    ITicketRepository tickets,
    IUnitOfWork unitOfWork)
    : IRequestHandler<UpdateTicketStatusCommand, Guid>
{
    private readonly ITicketRepository _tickets = tickets;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Guid> Handle(UpdateTicketStatusCommand request, CancellationToken cancellationToken)
    {
        var ticket = await _tickets.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new Exception("Ticket not found");

        if (!request.IsAdmin && ticket.UserId != request.UserId)
        {
            throw new Exception("Access denied: You cannot modify this ticket.");
        }

        if (request.NewStatus != ticket.Status)
        {
            switch (request.NewStatus)
            {
                case TicketStatus.Confirmed:
                    if (!request.IsAdmin && ticket.Status != TicketStatus.Pending)
                    {
                        throw new Exception("Users can only confirm Pending tickets (after payment).");
                    }
                    ticket.Confirm(); 
                    break;

                case TicketStatus.Cancelled:
                    ticket.Cancel();
                    break;

                case TicketStatus.Refunded:
                    if (!request.IsAdmin) 
                        throw new Exception("Only admin can refund tickets.");
                    
                    ticket.Refund();
                    break;

                default:
                    throw new Exception($"Transition to {request.NewStatus} is not supported via API.");
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        return ticket.Id.Id;
    }
}