using AbsoluteCinema.Application.Repository;
using AbsoluteCinema.Domain.Entities; 
using MediatR;

namespace AbsoluteCinema.Application.Features.Sessions.Commands.DeleteSession;

public record DeleteSessionCommand(Guid SessionId) : IRequest;

public class DeleteSessionCommandHandler : IRequestHandler<DeleteSessionCommand>
{
    private readonly ISessionRepository _repository;

    public DeleteSessionCommandHandler(ISessionRepository repository)
    {
        _repository = repository;
    }

    public async Task Handle(DeleteSessionCommand request, CancellationToken cancellationToken)
    {
        var sessionId = new SessionId(request.SessionId);

        var exists = await _repository.AnyAsync(sessionId, cancellationToken);

        if (!exists)
        {
            throw new KeyNotFoundException($"Session with id {request.SessionId} was not found.");
        }

        await _repository.DeleteAsync(sessionId, cancellationToken);

        await _repository.SaveChangesAsync(cancellationToken);
    }
}