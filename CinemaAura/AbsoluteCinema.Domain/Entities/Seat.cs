using CinemaAura.Domain.Primitives;

namespace AbsoluteCinema.Domain.Entities;

public class Seat : Entity<SeatId>
{
    //TODO : hallId
    public short Number { get; private set; }
    public SeatTypeId SeatTypeId { get; private set; }

    //TODO : HallId
    private Seat(Guid hallId, short number, SeatTypeId seatTypeId)
    {
        Id = SeatId.New();
        Number = number;
        SeatTypeId = seatTypeId;
    }

    //TODO : HallId
    public Seat Create(Guid hallId, short number, SeatTypeId seatTypeId)
    {
        return new Seat(hallId, number, seatTypeId);
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

public record SeatId(Guid Id)
{
    public static SeatId New() => new SeatId(Guid.NewGuid());
}