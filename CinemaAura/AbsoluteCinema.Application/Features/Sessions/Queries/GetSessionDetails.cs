using AbsoluteCinema.Application.Repository;
using AbsoluteCinema.Domain.Entities;
using MediatR;

namespace AbsoluteCinema.Application.Features.Sessions.Queries;

public record SessionDetailsDto(
    Guid Id,
    Guid MovieId,
    Guid HallId,
    DateTime StartTime,
    List<SessionPriceDto> Prices
);

public record SessionPriceDto(Guid SeatTypeId, decimal Price);

public record GetSessionDetailsQuery(Guid Id) : IRequest<SessionDetailsDto>;

public class GetSessionDetailsHandler : IRequestHandler<GetSessionDetailsQuery, SessionDetailsDto>
{
    private readonly ISessionRepository _repository;

    public GetSessionDetailsHandler(ISessionRepository repository)
    {
        _repository = repository;
    }

    public async Task<SessionDetailsDto> Handle(GetSessionDetailsQuery request, CancellationToken cancellationToken)
    {
        var sessionId = new SessionId(request.Id);

        var session = await _repository.GetByIdWithPricesAsync(sessionId, cancellationToken);

        if (session is null)
            throw new Exception($"Session {request.Id} not found");

        return new SessionDetailsDto(
            session.Id.Id,
            session.MovieId.Id,
            session.HallId.Id,
            session.StartDateTime,
            session.TypePrices.Select(tp => new SessionPriceDto(tp.SeatTypeId.Id, tp.Price)).ToList()
        );
    }
}