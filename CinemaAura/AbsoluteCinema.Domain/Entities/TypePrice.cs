using CinemaAura.Domain.Primitives;

namespace AbsoluteCinema.Domain.Entities;

public class TypePrice : Entity<TypePriceId>
{
    //TODO: public SessionId SessionId {get; pr set;}
    public TypePriceId TypePriceId { get; private set; }
    //public TypePrice TypePrice { get; private set; }
    
    public double Price { get; private set; }
}

public record TypePriceId(Guid Id)
{
    public static TypePriceId New() => new TypePriceId(Guid.NewGuid());
}