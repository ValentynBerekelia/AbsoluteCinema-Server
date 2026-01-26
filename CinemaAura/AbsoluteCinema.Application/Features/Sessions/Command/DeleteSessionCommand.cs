using MediatR;

namespace AbsoluteCinema.Application.Features.Sessions.Commands.DeleteSession;

public record DeleteSessionCommand(Guid SessionId) : IRequest;

