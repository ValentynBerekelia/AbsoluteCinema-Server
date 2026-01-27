using AbsoluteCinema.Domain.Entities;
using MediatR;

namespace AbsoluteCinema.Application.Features.Sessions.Commands.CreateSession;

public record CreateSessionCommand(
    Guid MovieId,
    Guid HallId,
    MovieFormat Format,
    DateTime StartTime,
    List<SessionPriceDto> Prices
) : IRequest<CreateSessionResponse>;

public record SessionPriceDto(
    Guid SeatTypeId,
    decimal Price
);