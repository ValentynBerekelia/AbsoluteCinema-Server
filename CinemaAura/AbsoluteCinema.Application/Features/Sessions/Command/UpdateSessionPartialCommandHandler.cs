
using AbsoluteCinema.Application.Repository;
using AbsoluteCinema.Domain.Entities;
using MediatR;

namespace AbsoluteCinema.Application.Features.Sessions.Commands;

public class UpdateSessionPartialCommandHandler : IRequestHandler<UpdateSessionPartialCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ISessionRepository _sessions;
    public UpdateSessionPartialCommandHandler(IUnitOfWork unit, ISessionRepository sessions)
    {
        _unitOfWork = unit;
        _sessions = sessions;
    }

    public async Task Handle(UpdateSessionPartialCommand command, CancellationToken ct)
    {
        var session = await _sessions.GetByIdAsync(command.Id, ct) 
            ?? throw new Exception("Session not found");

        if (command.MovieId.HasValue)
        {
            session.ChangeMovie(command.MovieId.Value);
        }

        if (command.HallId.HasValue)
        {
            session.ChangeHall(command.HallId.Value);
        }

        if (command.Format.HasValue)
        {
            session.ChangeFormat(command.Format.Value);
        }

        if (command.StartDateTime.HasValue)
        {
            session.Reschedule(command.StartDateTime.Value);
        }

        await _unitOfWork.SaveChangesAsync(ct);
    }
}

public record UpdateSessionPartialCommand(
    SessionId Id,
    MovieId? MovieId,
    HallId? HallId,
    MovieFormat? Format,
    DateTime? StartDateTime
) : IRequest;