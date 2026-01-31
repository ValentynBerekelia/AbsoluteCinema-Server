using AbsoluteCinema.Application.DTOs;
using AbsoluteCinema.Application.DTOs.User;
using AbsoluteCinema.Application.Repository;
using AbsoluteCinema.Domain.Entities;
using MediatR;
using static AbsoluteCinema.Application.Features.Tickets.Commands.CreateTicketCommandHandler;

namespace AbsoluteCinema.Application.Features.Tickets.Commands
{
    public class CreateTicketCommandHandler :
        IRequestHandler<CreateTicketCommand, CreateTicketResponse>
{
    private readonly ITicketRepository _ticketRepository;
    private readonly IUnitOfWork _unitOfWork;
    public CreateTicketCommandHandler(ITicketRepository ticket, IUnitOfWork unit)
    {
        _ticketRepository = ticket;
        _unitOfWork = unit;
    }
    public async Task<CreateTicketResponse> Handle(CreateTicketCommand request, CancellationToken ct)
    {
        var sessionId = new SessionId(request.SessionId);
        var seatId = new SeatId(request.SeatId);
            var userId = new UserId(request.UserId);
            var ticket = Ticket.Create(
                userId,
                sessionId,
                seatId
                );
            await _ticketRepository.AddAsync(ticket, ct);
            await _unitOfWork.SaveChangesAsync(ct);

            return new CreateTicketResponse(ticket.Id.Id);
        }

    public record CreateTicketCommand(
        Guid SessionId,
        Guid SeatId,
        Guid UserId
        ) : IRequest<CreateTicketResponse>;
    public record CreateTicketResponse(
        Guid Id
        );
    public record CreateTicketRequest(
        Guid SessionId,
        Guid SeatId,
        Guid? UserId
        );
}
}
