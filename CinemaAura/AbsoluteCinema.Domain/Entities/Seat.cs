using System.Formats.Tar;
using CinemaAura.Domain.Primitives;

namespace AbsoluteCinema.Domain.Entities;

public class Seat : Entity<SeatId>
{
    public short Number { get; private set; }
    public SeatTypeId SeatTypeId { get; private set; }

    private Seat() { }
    private Seat(short number, SeatTypeId seatTypeId)
    {
        Id = SeatId.New();
        Number = number;
        SeatTypeId = seatTypeId;
    }

    public static Seat Create(short number, SeatTypeId seatTypeId)
    {
        if (number <= 0)
        {
            throw new ArgumentException("Seat number must be positive.", nameof(number));
        }
        return new Seat(number, seatTypeId);
    }

    public void ChangeNumber(short number)
    {
        Number = number;
    }

    public void ChangeSeatTypeId(SeatTypeId id)
    {
        SeatTypeId = id;
    }
    
}

public record struct SeatId(Guid Id)
{
    public static SeatId New() => new SeatId(Guid.NewGuid());
}