using AbsoluteCinema.Application.Abstractions;
using AbsoluteCinema.Domain.Entities;

namespace AbsoluteCinema.Application.Repository;

public interface ITicketRepository : IRepository<TicketId, Ticket>
{
    Task<Ticket?> GetByIdWithDetailsAsync(TicketId id, CancellationToken ct = default);
}