using AbsoluteCinema.Application.Repository;
using AbsoluteCinema.Domain.Entities;
using MediatR;

namespace AbsoluteCinema.Application.Features.Tickets.Commands;

public record ConfirmTicketCommand(Guid TicketId) : IRequest;

public class ConfirmTicketCommandHandler : IRequestHandler<ConfirmTicketCommand>
{
    private readonly ITicketRepository _ticketRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ConfirmTicketCommandHandler(ITicketRepository ticketRepository, IUnitOfWork unitOfWork)
    {
        _ticketRepository = ticketRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(ConfirmTicketCommand request, CancellationToken ct)
    {
        var ticketId = new TicketId(request.TicketId);

        var ticket = await _ticketRepository.GetByIdAsync(ticketId, ct);

        if (ticket is null)
        {
            throw new Exception($"Ticket with id {request.TicketId} not found.");
        }

        ticket.Confirm();

        _ticketRepository.Update(ticket);

        await _unitOfWork.SaveChangesAsync(ct);
    }
}