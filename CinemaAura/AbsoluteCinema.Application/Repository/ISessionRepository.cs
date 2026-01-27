using AbsoluteCinema.Application.Abstractions;
using AbsoluteCinema.Domain.Entities;
using AbsoluteCinema.Domain.Enums;

namespace AbsoluteCinema.Application.Repository;

public interface ISessionRepository : IRepository<SessionId, Session>
{
    Task SaveChangesAsync(CancellationToken ct = default);

    Task<Session?> GetByIdWithPricesAsync(SessionId id, CancellationToken ct = default);

    Task<List<Session>> GetAllWithDetailsAsync(CancellationToken ct = default);

    Task<(List<Session> Items, int TotalCount)> GetPagedSessionsAsync(
        Guid movieId,
        int pageNumber,
        int pageSize,
        string? sortColumn,
        SortOrder sortOrder,
        CancellationToken ct = default);
}