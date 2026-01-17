using CinemaAura.Domain.Primitives;

namespace AbsoluteCinema.Domain.Entities;

public class Ticket : Entity<TicketId>
{
    public UserId? UserId { get; private set; }
    public SessionId SessionId { get; private set; }
    public SeatId SeatId { get; private set; }

    // NOTE: UNIQUE constraint (SessionId, SeatId) setup in configuration to prevent double booking
    
    public TicketStatus Status { get; private set; }
    public DateTime PurchasedAt { get; private set; }
    private Ticket() { }
    private Ticket(TicketId id, UserId? userId, SessionId sessionId, SeatId seatId,  DateTime purchasedAt)
    {
        Id = id;
        UserId = userId;
        SessionId = sessionId;
        SeatId = seatId;
        Status = TicketStatus.Pending;
        PurchasedAt = purchasedAt;
    }

    public static Ticket Create(UserId? userId, SessionId sessionId, SeatId seatId, decimal pricePaid)
    {
        if (pricePaid < 0)
            throw new ArgumentException("Price cannot be negative.", nameof(pricePaid));

        return new Ticket(
            TicketId.New(),
            userId,
            sessionId,
            seatId,
            DateTime.UtcNow
        );
    }
    public void Confirm()
    {
        if (Status != TicketStatus.Pending)
            throw new InvalidOperationException("Only pending tickets can be confirmed.");

        Status = TicketStatus.Confirmed;
    }

    public void Cancel()
    {
        if (Status == TicketStatus.Refunded)
            throw new InvalidOperationException("Refunded tickets cannot be cancelled.");

        Status = TicketStatus.Cancelled;
    }

    public void Refund()
    {
        if (Status != TicketStatus.Confirmed)
            throw new InvalidOperationException("Only confirmed tickets can be refunded.");

        Status = TicketStatus.Refunded;
    }
    public void ChangeSeat(SeatId newSeatId)
    {
        if (Status != TicketStatus.Pending)
            throw new InvalidOperationException("Cannot change seat after confirmation.");

        SeatId = newSeatId;
    }

    public void ChangeSession(SessionId newSessionId)
    {
        SessionId = newSessionId;
    }
}

public enum TicketStatus
{
    Pending,    // in basket, wait for payment
    Confirmed,  // paid
    Cancelled,  // cancelled by user
    Refunded    // refunded money (only after being confirmed)
}

public record struct TicketId(Guid Id)
{
    public static TicketId New() => new TicketId(Guid.NewGuid());
}
