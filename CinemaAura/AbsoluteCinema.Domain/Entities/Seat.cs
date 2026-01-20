using AbsoluteCinema.Domain.Primitives;

namespace AbsoluteCinema.Domain.Entities;

public class Seat : Entity<SeatId>
{
    public HallId HallId { get; private set; }
    public short Row { get; private set; }
    public short Number { get; private set; }
    public SeatTypeId SeatTypeId { get; private set; }

    private Seat() { }
    private Seat(SeatId id, HallId hallId, short row, short number, SeatTypeId seatTypeId)
    {
        Id = id;
        HallId = hallId;
        Row = row;
        Number = number;
        SeatTypeId = seatTypeId;
    }

    public static Seat Create(HallId hallId, short row, short number, SeatTypeId seatTypeId)
    {
        if (row <= 0)
        {
            throw new ArgumentException("Row must be positive.", nameof(row));
        }

        if (number <= 0)
        {
            throw new ArgumentException("Seat number must be positive.", nameof(number));
        }

        return new Seat(SeatId.New(), hallId, row, number, seatTypeId);
    }

    public void ChangeRow(short row)
    {
        if (row <= 0) throw new ArgumentException("Row must be positive.");
        Row = row;
    }

    public void ChangeNumber(short number)
    {
        if (number <= 0) throw new ArgumentException("Number must be positive.");
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