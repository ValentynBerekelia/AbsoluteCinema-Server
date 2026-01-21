using AbsoluteCinema.Domain.Exceptions;
using AbsoluteCinema.Domain.Primitives;

namespace AbsoluteCinema.Domain.Entities;

public class Session : AggregateRoot<SessionId>
{
    public MovieId MovieId { get; private set; }
    public Movie Movie { get; private set; } 

    public HallId HallId { get; private set; }
    public Hall Hall { get; private set; }
    public DateTime StartDateTime { get; private set; }

    private readonly List<Ticket> _tickets = new();
    public IReadOnlyCollection<Ticket> Tickets => _tickets.AsReadOnly();

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
            throw new DomainException("Session start date must be in the future.");
        }

        return new Session(SessionId.New(), movieId, hallId, date);
    }
    public void Reschedule(DateTime newDate)
    {
        if (newDate < DateTime.UtcNow)
        {
            throw new DomainException("New session date must be in the future.");
        }
        StartDateTime = newDate;
    }
    

    public void ChangeMovie(MovieId newMovieId)
    {
        MovieId = newMovieId;
    }
    public void ChangeHall(HallId newHallId)
    {
        if (_tickets.Any())
            throw new DomainException("Cannot change hall after tickets have been sold.");

        HallId = newHallId;
    }

    public void AddTicket(Ticket ticket)
    {
        if (_tickets.Any(t => t.SeatId == ticket.SeatId))
            throw new DomainException("This seat is already taken.");

        _tickets.Add(ticket);
    }
}

public record struct SessionId(Guid Id)
{
    public static SessionId New() => new SessionId(Guid.NewGuid());
}