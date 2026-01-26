using AbsoluteCinema.Application.Repository;
using AbsoluteCinema.Domain.Entities;
using MediatR;

namespace AbsoluteCinema.Application.Features.Sessions.Commands.CreateSession;

public record CreateSessionCommand(
    Guid MovieId,
    string HallName,
    DateTime StartTime
) : IRequest<CreateSessionResponse>;

public class CreateSession : IRequestHandler<CreateSessionCommand, CreateSessionResponse>
{
    private readonly ISessionRepository _sessionRepository;
    private readonly IHallRepository _hallRepository;

    // Ін'єктимо інтерфейси
    public CreateSession(ISessionRepository sessionRepository, IHallRepository hallRepository)
    {
        _sessionRepository = sessionRepository;
        _hallRepository = hallRepository;
    }

    public async Task<CreateSessionResponse> Handle(CreateSessionCommand request, CancellationToken cancellationToken)
    {
        var hall = await _hallRepository.GetByNameAsync(request.HallName, cancellationToken);

        if (hall is null)
        {
            throw new KeyNotFoundException($"Hall with name '{request.HallName}' not found.");
        }

        var session = Session.Create(
            new MovieId(request.MovieId),
            hall.Id,
            request.StartTime
        );

        await _sessionRepository.AddAsync(session, cancellationToken);
        await _sessionRepository.SaveChangesAsync(cancellationToken);

        return new CreateSessionResponse(session.Id.Id);
    }
}

public record CreateSessionResponse(Guid Id);