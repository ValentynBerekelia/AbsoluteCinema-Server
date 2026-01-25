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


    private readonly List<TypePrice> _typePrices = new();
    public IReadOnlyCollection<TypePrice> TypePrices => _typePrices.AsReadOnly();

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

    public void UpdateDetails(MovieId newMovieId, HallId newHallId, DateTime newDate)
    {
        ChangeMovie(newMovieId);
        ChangeHall(newHallId);
        Reschedule(newDate);
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

    public void AddPrice(TypePrice price)
    {
        if (_typePrices.Any(p => p.SeatTypeId == price.SeatTypeId))
        {
            // you can either ignore, throw an exception, or update
            // I thought there should be a checkbox for VIP and the first 2 rows, for example,
            // then you can throw an exception on them and add the code here
            return;
        }
        _typePrices.Add(price);
    }

    public void RemovePrice(TypePrice price)
    {
        _typePrices.Remove(price);
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

    public void CancelTicket(TicketId ticketId)
    {
        _ticketIds.Remove(ticketId);
    }
}

public record struct SessionId(Guid Id)
{
    public static SessionId New() => new SessionId(Guid.NewGuid());
}