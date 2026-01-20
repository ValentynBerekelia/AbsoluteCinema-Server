using System.ComponentModel.DataAnnotations.Schema;

namespace AbsoluteCinema.Domain.Primitives;

public abstract class AggregateRoot<TId> :Entity<TId> where TId : notnull
{
    protected AggregateRoot() : base()
    {}
    protected AggregateRoot(TId id) : base(id)
    {
    }

    private readonly List<IDomainEvent> _domainEvents = new();
    [NotMapped]
    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents;

    public void Raise(IDomainEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }

    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }
}