using MediatR;

namespace CinemaAura.Application.Features.Sessions.Commands.CreateSession;

public record CreateSessionCommand(
    Guid MovieId,
    Guid HallId,
    DateTime StartTime,
    List<SessionPriceDto> Prices
) : IRequest<Guid>;

public record SessionPriceDto(
    Guid SeatTypeId,
    decimal Price
);