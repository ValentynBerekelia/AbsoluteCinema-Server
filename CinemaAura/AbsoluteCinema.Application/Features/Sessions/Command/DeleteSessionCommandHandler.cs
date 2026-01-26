using AbsoluteCinema.Application.Repository;
using AbsoluteCinema.Domain.Entities;
using AbsoluteCinema.Domain.Exceptions;
using MediatR;

namespace AbsoluteCinema.Application.Features.Sessions.Commands.DeleteSession;

public class DeleteSessionCommandHandler : IRequestHandler<DeleteSessionCommand>
{
    private readonly ISessionRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteSessionCommandHandler(ISessionRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(DeleteSessionCommand request, CancellationToken cancellationToken)
    {
        var sessionId = new SessionId(request.SessionId);
        
        var session = await _repository.GetByIdAsync(sessionId, cancellationToken);
        if (session is null)
        {
            throw new DomainException("Session not found");
        }

        await _repository.DeleteAsync(sessionId, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}

