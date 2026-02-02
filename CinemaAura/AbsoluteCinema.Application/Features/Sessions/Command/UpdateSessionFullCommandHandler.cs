using AbsoluteCinema.Application.Repository;
using AbsoluteCinema.Domain.Entities;
using MediatR;

namespace AbsoluteCinema.Application.Features.Sessions.Commands;

public class UpdateSessionFullCommandHandler : IRequestHandler<UpdateSessionFullCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ISessionRepository _sessions;

    public UpdateSessionFullCommandHandler(IUnitOfWork unit, ISessionRepository sessions)
    {
        _unitOfWork = unit;
        _sessions = sessions;
    }

    public async Task Handle(UpdateSessionFullCommand command, CancellationToken ct)
    {
        var session = await _sessions.GetByIdWithPricesAsync(command.Id, ct)
            ?? throw new Exception("Session not found");

        session.ChangeMovie(command.MovieId);
        session.ChangeHall(command.HallId);
        session.ChangeFormat(command.Format);
        session.Reschedule(command.StartDateTime);
        session.ChangePrices(command.SeatPrices.Select(p => 
            TypePrice.Create(session.Id, new SeatTypeId(p.Key), p.Value)));

        await _unitOfWork.SaveChangesAsync(ct);
    }
}

public record UpdateSessionFullCommand(
    SessionId Id,
    MovieId MovieId,
    HallId HallId,
    MovieFormat Format,
    DateTime StartDateTime,
    Dictionary<Guid, decimal> SeatPrices
) : IRequest;