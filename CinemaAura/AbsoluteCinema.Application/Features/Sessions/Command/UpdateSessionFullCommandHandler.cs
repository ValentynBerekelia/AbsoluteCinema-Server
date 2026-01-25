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
        var session = await _sessions.GetByIdAsync(new SessionId(command.Id), ct)
            ?? throw new Exception("Session not found");

        session.ChangeMovie(new MovieId(command.MovieId));
        session.ChangeHall(new HallId(command.HallId));
        session.ChangeFormat(command.Format);
        session.Reschedule(command.StartDateTime);

        await _unitOfWork.SaveChangesAsync(ct);
    }
}

public record UpdateSessionFullCommand(
    Guid Id,
    Guid MovieId,
    Guid HallId,
    MovieFormat Format,
    DateTime StartDateTime
) : IRequest;