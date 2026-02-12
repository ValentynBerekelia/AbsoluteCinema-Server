using AbsoluteCinema.Application.Abstractions;
using AbsoluteCinema.Application.Common;
using AbsoluteCinema.Application.Repository;
using AbsoluteCinema.Domain.Entities;
using MediatR;

namespace AbsoluteCinema.Application.Features.Tickets.Commands;

public record ConfirmTicketCommand(Guid TicketId) : IRequest;

public class ConfirmTicketCommandHandler : IRequestHandler<ConfirmTicketCommand>
{
    private readonly ITicketRepository _ticketRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IEmailService _emailService;

    public ConfirmTicketCommandHandler(
        ITicketRepository ticketRepository,
        IUnitOfWork unitOfWork,
        IEmailService emailService) 
    {
        _ticketRepository = ticketRepository;
        _unitOfWork = unitOfWork;
        _emailService = emailService;
    }

    public async Task Handle(ConfirmTicketCommand request, CancellationToken ct)
    {
        var ticketId = new TicketId(request.TicketId);

        var ticket = await _ticketRepository.GetByIdWithDetailsAsync(ticketId, ct);

        if (ticket is null)
        {
            throw new Exception($"Ticket with id {request.TicketId} not found.");
        }

        ticket.Confirm();
        _ticketRepository.Update(ticket);
        await _unitOfWork.SaveChangesAsync(ct);

        try
        {
            if (ticket.Session?.Movie == null || ticket.User == null)
            {
                return;
            }

            var pdfBytes = PdfTicketGenerator.GenerateTicket(
                ticket.Session.Movie.Name,
                ticket.Session.Hall.HallName,
                ticket.Seat.Row,
                ticket.Seat.Number,
                ticket.Session.StartDateTime,
                ticket.Id.Id
            );

            var emailBody = $@"
                <div style='font-family: Arial, sans-serif; color: #333;'>
                    <h2 style='color: #d32f2f;'>Absolute Cinema</h2>
                    <h3>Ваше замовлення підтверджено!</h3>
                    <p>Дякуємо, що обрали нас.</p>
                    <hr/>
                    <p><strong>Фільм:</strong> {ticket.Session.Movie.Name}</p>
                    <p><strong>Дата:</strong> {ticket.Session.StartDateTime:dd.MM.yyyy HH:mm}</p>
                    <p><strong>Місце:</strong> Ряд {ticket.Seat.Row}, Місце {ticket.Seat.Number}</p>
                    <br/>
                    <p>📎 <strong>Ваш квиток знаходиться у вкладенні.</strong></p>
                    <p>Покажіть QR-код контролеру при вході в зал.</p>
                </div>
            ";

            await _emailService.SendEmailWithAttachmentAsync(
                ticket.User.Email,
                $"Ваш квиток: {ticket.Session.Movie.Name}",
                emailBody,
                pdfBytes,
                $"ticket-{ticket.Id.Id}.pdf" 
            );
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[Email Error] Failed to send ticket: {ex.Message}");
        }
    }
}