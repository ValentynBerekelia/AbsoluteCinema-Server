using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CinemaAura.Domain.Primitives;

namespace IdentityService.Domain.Entities;

public class Hall : Entity<HallId>
{
    public int VerticalSize { get; private set; }
    public int HorizontalSize { get; private set; }

    private readonly HashSet<SeatId> _seatIds = new HashSet<SeatId>();
    public IReadOnlyCollection<SeatId> SeatIds => _seatIds;
    private readonly HashSet<SessionId> _sessionIds = new HashSet<SessionId>();
    public IReadOnlyCollection<SessionId> SessionIds => _sessionIds;

    private Hall(HallId id, int verticalSize, int horizontalSize)
    {
        Id = id;
        VerticalSize = verticalSize;
        HorizontalSize = horizontalSize;
    }
    public static Hall Create(int verticalSize, int horizontalSize)
    {
        if (verticalSize <= 0 || horizontalSize <= 0)
        {
            throw new ArgumentException("Hall dimensions must be positive.");
        }
        return new Hall(HallId.New(), verticalSize, horizontalSize);
    }
    public void Resize(int verticalSize, int horizontalSize)
    {
        if (verticalSize <= 0 || horizontalSize <= 0)
        {
            throw new ArgumentException("Hall dimensions must be positive.");
        }
        VerticalSize = verticalSize;
        HorizontalSize = horizontalSize;
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
public record HallId(Guid Id)
{
    public static HallId New() => new HallId(Guid.NewGuid());
}
