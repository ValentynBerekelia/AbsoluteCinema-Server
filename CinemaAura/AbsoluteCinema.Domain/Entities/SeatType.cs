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

    public static SeatType Create(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Seat type name cannot be null or empty.", nameof(name));
        }
        return new SeatType(name);
    }

    public void ChangeTypeName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Seat type name cannot be null or empty.", nameof(name));
        }
        TypeName = name;
    }
}

public record SeatTypeId(Guid Id)
{
    public static SeatTypeId New() => new SeatTypeId(Guid.NewGuid());
}