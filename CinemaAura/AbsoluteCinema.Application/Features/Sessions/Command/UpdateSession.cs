using AbsoluteCinema.Application.Repository;
using AbsoluteCinema.Domain.Entities;
using MediatR;

namespace AbsoluteCinema.Application.Features.Sessions.Commands.UpdateSession;

public record UpdateSessionCommand(
    Guid SessionId,
    Guid? MovieId,
    Guid? HallId,
    DateTime? StartTime
) : IRequest;

public class UpdateSessionHandler : IRequestHandler<UpdateSessionCommand>
{
    private readonly ISessionRepository _sessionRepository;
    private readonly IHallRepository _hallRepository;

    public UpdateSessionHandler(ISessionRepository sessionRepository, IHallRepository hallRepository)
    {
        _sessionRepository = sessionRepository;
        _hallRepository = hallRepository;
    }

    public async Task Handle(UpdateSessionCommand request, CancellationToken cancellationToken)
    {
        var sessionId = new SessionId(request.SessionId);

        var session = await _sessionRepository.GetByIdAsync(sessionId, cancellationToken);

        if (session is null)
            throw new KeyNotFoundException($"Session {request.SessionId} not found");

        if (request.MovieId.HasValue)
        {
            session.ChangeMovie(new MovieId(request.MovieId.Value));
        }

        if (request.StartTime.HasValue)
        {
            session.Reschedule(request.StartTime.Value);
        }
        if (request.HallId.HasValue)
        {
            session.ChangeHall(new HallId(request.HallId.Value));
        }

        await _sessionRepository.SaveChangesAsync(cancellationToken);
    }
}