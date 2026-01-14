using CinemaAura.Domain.Primitives;
using IdentityService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaAura.Domain.Entities
{
    public class Ticket : Entity<TicketId>
    {
        public UserId UserId { get; private set; }
        public SessionId SessionId { get; private set; }
        public SeatId SeatId { get; private set; }
        public DateTime Date { get; private set; }
        private Ticket(TicketId id, UserId userId, SessionId sessionId, SeatId seatId, DateTime date)
        {
            Id = id;
            UserId = userId;
            SessionId = sessionId;
            SeatId = seatId;
            Date = date;
        }
        public static Ticket Create(UserId userId, SessionId sessionId, SeatId seatId, DateTime date)
        {
            return new Ticket(TicketId.New(), userId, sessionId, seatId, date);
        }

        public void ChangeSeat(SeatId newSeatId)
        {
            SeatId = newSeatId;
        }

        public void ChangeSession(SessionId newSessionId)
        {
            SessionId = newSessionId;
        }

        public void ChangeDate(DateTime newDate)
        {
            Date = newDate;
        }
    }
}
public record TicketId(Guid Id)
{
    public static TicketId New() => new TicketId(Guid.NewGuid());
}
