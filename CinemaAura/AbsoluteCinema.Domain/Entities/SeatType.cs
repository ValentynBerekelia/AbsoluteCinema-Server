using CinemaAura.Domain.Primitives;

namespace AbsoluteCinema.Domain.Entities;

public class SeatType : AggregateRoot<SeatTypeId>
{
    public string TypeName { get; private set; }

    private SeatType(string name)
    {
        Id = SeatTypeId.New();
        TypeName = name;
    }

    public SeatType Create(string name)
    {
        return new SeatType(name);
    }

    public void ChangeTypeName(string name)
    {
        TypeName = name;
    }
}

public record SeatTypeId(Guid Id)
{
    public static SeatTypeId New() => new SeatTypeId(Guid.NewGuid());
}