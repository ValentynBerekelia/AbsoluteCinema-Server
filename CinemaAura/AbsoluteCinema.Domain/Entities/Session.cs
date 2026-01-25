using AbsoluteCinema.Domain.Exceptions;
using AbsoluteCinema.Domain.Primitives;

namespace AbsoluteCinema.Domain.Entities;

public class Session : AggregateRoot<SessionId>
{
    public MovieId MovieId { get; private set; }
    public HallId HallId { get; private set; }
    public DateTime StartDateTime { get; private set; }

    private readonly HashSet<TicketId> _ticketIds = new HashSet<TicketId>();
    public IReadOnlyCollection<TicketId> TicketIds => _ticketIds;


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

    public void ChangeHall(HallId newHallId)
    {
        HallId = newHallId;
    }

    public void ChangeMovie(MovieId newMovieId)
    {
        MovieId = newMovieId;
    }

    public void AddPrice(TypePrice price)
    {
        if (_typePrices.Any(p => p.SeatTypeId == price.SeatTypeId))
        {
            // можна або ігнорувати, або кидати ексепшн, або оновлювати
            // я думала, щоб на віпку і перші 2 ряди був чекбокс, наприклад,
            // тоді можна буде поставити ексепшн на них і дописати тут код
            return;
        }
        _typePrices.Add(price);
    }

    public void RemovePrice(TypePrice price)
    {
        _typePrices.Remove(price);
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