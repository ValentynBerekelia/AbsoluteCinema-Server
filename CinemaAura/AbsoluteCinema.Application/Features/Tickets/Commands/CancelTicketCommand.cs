using AbsoluteCinema.Application.Repository;
using AbsoluteCinema.Domain.Entities;
using MediatR;

namespace AbsoluteCinema.Application.Features.Tickets.Commands;

public record CancelTicketCommand(Guid TicketId, Guid UserId) : IRequest;

public class CancelTicketCommandHandler : IRequestHandler<CancelTicketCommand>
{
    private readonly ITicketRepository _ticketRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CancelTicketCommandHandler(ITicketRepository ticketRepository, IUnitOfWork unitOfWork)
    {
        _ticketRepository = ticketRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(CancelTicketCommand request, CancellationToken ct)
    {
        var ticketId = new TicketId(request.TicketId);

        var ticket = await _ticketRepository.GetByIdAsync(ticketId, ct);

        if (ticket is null)
        {
            throw new Exception("Ticket not found");
        }

        // Змініть рядок 30 на цей:
        if (ticket.UserId?.Id != request.UserId)
        {
            throw new Exception("Access denied: You can only cancel your own tickets.");
        }

        await _ticketRepository.DeleteAsync(ticketId, ct);

        await _unitOfWork.SaveChangesAsync(ct);
    }
}