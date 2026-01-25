using AbsoluteCinema.Application.Repository;
using AbsoluteCinema.Domain.Entities;
using MediatR;

namespace AbsoluteCinema.Application.Features.Sessions.Commands.CreateSession;

public record CreateSessionCommand(
    Guid MovieId,
    Guid HallId,
    DateTime StartTime,
    List<SessionPriceDto> Prices
) : IRequest<CreateSessionResponse>;

public record SessionPriceDto(
    Guid SeatTypeId,
    decimal Price
);

public class CreateSession : IRequestHandler<CreateSessionCommand, CreateSessionResponse>
{
    private readonly ISessionRepository _repository;

    public CreateSession(ISessionRepository repository)
    { _repository = repository; }

    public async Task<CreateSessionResponse> Handle(CreateSessionCommand request, CancellationToken cancellationToken)
    {
        var session = Session.Create(
            new MovieId(request.MovieId),
            new HallId(request.HallId),
            request.StartTime
        );

        foreach (var priceDto in request.Prices)
        {
            var typePrice = TypePrice.Create(
                session.Id,
                new SeatTypeId(priceDto.SeatTypeId),
                priceDto.Price
            );

            session.AddPrice(typePrice);
        }

        await _repository.AddAsync(session, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);

        return new CreateSessionResponse(session.Id.Id);
    }
}

public record CreateSessionResponse(Guid Id);