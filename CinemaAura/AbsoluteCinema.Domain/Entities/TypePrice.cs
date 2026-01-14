using CinemaAura.Domain.Primitives;

namespace AbsoluteCinema.Domain.Entities;

public class TypePrice : Entity<TypePriceId>
{
    public SessionId SessionId { get; private set; }
    public SeatTypeId SeatTypeId { get; private set; }
    public decimal Price { get; private set; }

    private TypePrice(TypePriceId id, SessionId sessionId, SeatTypeId seatTypeId, decimal price)
    {
        Id = id;
        SessionId = sessionId;
        SeatTypeId = seatTypeId;
        Price = price;
    }

    public static TypePrice Create(SessionId sessionId, SeatTypeId seatTypeId, decimal price)
    {
        if (price < 0)
        {
            throw new ArgumentException("Price cannot be negative.", nameof(price));
        }
        return new TypePrice(TypePriceId.New(), sessionId, seatTypeId, price);
    }

    public void ChangePrice(decimal newPrice)
    {
        if (newPrice < 0)
        {
            throw new ArgumentException("Price cannot be negative.", nameof(newPrice));
        }
        Price = newPrice;
    }

    public void ChangeSession(SessionId newSessionId)
    {
        SessionId = newSessionId;
    }

    public void ChangeSeatType(SeatTypeId newSeatTypeId)
    {
        SeatTypeId = newSeatTypeId;
    }
}

public record TypePriceId(Guid Id)
{
    public static TypePriceId New() => new TypePriceId(Guid.NewGuid());
}