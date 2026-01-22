using AbsoluteCinema.Domain.Primitives;

namespace AbsoluteCinema.Domain.Entities;

public class Hall : AggregateRoot<HallId>
{
    public string HallName { get; private set; }
    public int VerticalSize { get; private set; }
    public int HorizontalSize { get; private set; }

    private readonly HashSet<SeatId> _seatIds = new HashSet<SeatId>();
    public IReadOnlyCollection<SeatId> SeatIds => _seatIds;
    private readonly HashSet<SessionId> _sessionIds = new HashSet<SessionId>();
    public IReadOnlyCollection<SessionId> SessionIds => _sessionIds;
    private Hall() { }

    private Hall(HallId id, string name)
    {
        Id = id;
        HallName = name;
        VerticalSize = verticalSize;
        HorizontalSize = horizontalSize;
    }
    public static Hall Create(string name, int verticalSize, int horizontalSize)
    {
        if (verticalSize <= 0 || horizontalSize <= 0)
        {
            throw new ArgumentException("Hall dimensions must be positive.");
        }
        return new Hall(HallId.New(), name);
    }

    public void ChangeHallName(string name)
    {
        HallName = name;
    }
    
    public void Resize(int? verticalSize, int? horizontalSize)
    {
        if (verticalSize is not null && verticalSize > 0)
        {
            VerticalSize = (int)verticalSize;
        }
        if (horizontalSize is not null && horizontalSize > 0)
        {
            HorizontalSize = (int)horizontalSize;
        }
    }
    public void AddSeat(SeatId seatId)
    {
        _seatIds.Add(seatId);
    }
    public void RemoveSeat(SeatId seatId)
    {
        _seatIds.Remove(seatId);
    }
}
public record struct HallId(Guid Id)
{
    public static HallId New() => new HallId(Guid.NewGuid());
}
