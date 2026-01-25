using AbsoluteCinema.Application.Repository;
using AbsoluteCinema.Domain.Entities;
using MediatR;

namespace AbsoluteCinema.Application.Features.Sessions.Commands.UpdateSession;

// Command
public record UpdateSessionCommand(
    Guid SessionId,
    Guid MovieId,
    Guid HallId,
    DateTime StartTime,
    List<UpdateSessionPriceDto> Prices
) : IRequest;

public record UpdateSessionPriceDto(Guid SeatTypeId, decimal Price);

// Handler
public class UpdateSessionHandler : IRequestHandler<UpdateSessionCommand>
{
    private readonly ISessionRepository _repository;

    public UpdateSessionHandler(ISessionRepository repository)
    {
        _repository = repository;
    }

    public async Task Handle(UpdateSessionCommand request, CancellationToken cancellationToken)
    {
        var sessionId = new SessionId(request.SessionId);
        // this method returns the session that EF is tracking
        var session = await _repository.GetByIdWithPricesAsync(sessionId, cancellationToken);

        if (session is null)
            throw new Exception($"Session {request.SessionId} not found");

        session.UpdateDetails(
            new MovieId(request.MovieId),
            new HallId(request.HallId),
            request.StartTime
        );

        var pricesToDelete = session.TypePrices
            .Where(existing => !request.Prices.Any(p => p.SeatTypeId == existing.SeatTypeId.Id))
            .ToList();

        // instead of calling the repository, we work through the Domain
        foreach (var price in pricesToDelete)
        {
            // we call the entity method that removes the price from the internal _typePrices collection
            session.RemovePrice(price);
        }

        foreach (var priceDto in request.Prices)
        {
            var existingPrice = session.TypePrices
                .FirstOrDefault(p => p.SeatTypeId.Id == priceDto.SeatTypeId);

            if (existingPrice != null)
            {
                existingPrice.ChangePrice(priceDto.Price);
            }
            else
            {
                var newPrice = TypePrice.Create(
                    session.Id,
                    new SeatTypeId(priceDto.SeatTypeId),
                    priceDto.Price
                );
                // add the price to the session collection, EF Core will see the new entity and add it
                session.AddPrice(newPrice);
            }
        }

        _repository.Update(session);

        await _repository.SaveChangesAsync(cancellationToken);
    }
}