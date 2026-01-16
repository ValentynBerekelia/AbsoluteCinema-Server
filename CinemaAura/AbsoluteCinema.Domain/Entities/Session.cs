using CinemaAura.Domain.Primitives;

namespace AbsoluteCinema.Domain.Entities;

public class Session : AggregateRoot<SessionId>
{
    public MovieId MovieId { get; private set; }
    public HallId HallId { get; private set; }
    public DateTime StartDateTime { get; private set; }

    private readonly HashSet<TicketId> _ticketIds = new HashSet<TicketId>();
    public IReadOnlyCollection<TicketId> TicketIds => _ticketIds;

    private Session() { }
    private Session(SessionId id, MovieId movieId, HallId hallId, DateTime date)
    {
        Id = id;
        MovieId = movieId;
        HallId = hallId;
        StartDateTime = date;
    }
    public static Session Create(MovieId movieId, HallId hallId, DateTime date)
    {
        if (date < DateTime.UtcNow)
        {
            throw new ArgumentException("Session date cannot be in the past.");
        }

        return new Session(SessionId.New(), movieId, hallId, date);
    }
    public void Reschedule(DateTime newDate)
    {
        if (newDate < DateTime.UtcNow)
        {
            throw new ArgumentException("New session date cannot be in the past.");
        }
        StartDateTime = newDate;
    }

    public void ChangeHall(HallId newHallId)
    {
        HallId = newHallId;
    }

    public void ChangeMovie(MovieId newMovieId)
    {
        MovieId = newMovieId;
    }
    public void AddTicket(TicketId ticketId)
    {
        _ticketIds.Add(ticketId);
    }
    public void CancelTicket(TicketId ticketId)
    {
        _ticketIds.Remove(ticketId);
    }
}

public record struct SessionId(Guid Id)
{
    public static SessionId New() => new SessionId(Guid.NewGuid());
}