﻿using AbsoluteCinema.Application.Repository;
using AbsoluteCinema.Domain.Entities;
using MediatR;

namespace AbsoluteCinema.Application.Features.Sessions.Commands.CreateSession;

public record CreateSessionCommand(
    Guid MovieId,
    Guid HallId,
    DateTime StartTime,
    MovieFormat Format
) : IRequest<CreateSessionResponse>;

public class CreateSession : IRequestHandler<CreateSessionCommand, CreateSessionResponse>
{
    private readonly ISessionRepository _sessionRepository;
    private readonly IHallRepository _hallRepository;

    public CreateSession(ISessionRepository sessionRepository, IHallRepository hallRepository)
    {
        _sessionRepository = sessionRepository;
        _hallRepository = hallRepository;
    }

    public async Task<CreateSessionResponse> Handle(CreateSessionCommand request, CancellationToken cancellationToken)
    {
        var hall = await _hallRepository.GetByIdAsync(new HallId(request.HallId), cancellationToken);

        if (hall is null)
        {
            throw new KeyNotFoundException($"Hall with name '{request.HallId}' not found.");
        }

        var session = Session.Create(
            new MovieId(request.MovieId),
            hall.Id,
            request.StartTime,
            request.Format
        );

        await _sessionRepository.AddAsync(session, cancellationToken);
        await _sessionRepository.SaveChangesAsync(cancellationToken);

        return new CreateSessionResponse(session.Id.Id);
    }
}

public record CreateSessionResponse(Guid Id);