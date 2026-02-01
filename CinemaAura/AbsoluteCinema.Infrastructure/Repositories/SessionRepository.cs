using AbsoluteCinema.Application.Repository;
using AbsoluteCinema.Domain.Entities;
using AbsoluteCinema.Domain.Enums;
using AbsoluteCinema.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace AbsoluteCinema.Infrastructure.Repositories;

public class SessionRepository : BaseRepository<SessionId, Session, CinemaDbContext>, ISessionRepository
{
    public SessionRepository(CinemaDbContext dbContext) : base(dbContext)
    {
    }

    public async Task SaveChangesAsync(CancellationToken ct = default)
    {
        await _dbContext.SaveChangesAsync(ct);
    }

    //one session with prices
    public async Task<Session?> GetByIdWithPricesAsync(SessionId id, CancellationToken ct = default)
    {
        return await _set // _set from BaseRepository
            .Include(s => s.TypePrices)
            .Include( s=> s.Tickets)
            .FirstOrDefaultAsync(s => s.Id == id, ct);
    }

    public async Task<List<Session>> GetAllWithDetailsAsync(CancellationToken ct = default)
    {
        return await _set
            .AsNoTracking()
            .Include(s => s.TypePrices)
            .ToListAsync(ct);
    }

    public async Task<(List<Session> Items, int TotalCount)> GetPagedSessionsAsync(
        Guid movieId,
        int pageNumber,
        int pageSize,
        string? sortColumn,
        SortOrder sortOrder,
        CancellationToken ct = default)
    {
        IQueryable<Session> query = _set
            .Include(s => s.Hall)
            .Where(s => s.MovieId == new MovieId(movieId))
            .AsNoTracking();

        bool isDesc = sortOrder == SortOrder.Desc;

        query = sortColumn?.ToLower() switch
        {
            "hallname" => isDesc ? query.OrderByDescending(s => s.Hall.HallName) : query.OrderBy(s => s.Hall.HallName),
            "startdatetime" => isDesc ? query.OrderByDescending(s => s.StartDateTime) : query.OrderBy(s => s.StartDateTime),
            _ => isDesc ? query.OrderByDescending(s => s.StartDateTime) : query.OrderBy(s => s.StartDateTime)
        };
        int totalCount = await query.CountAsync(ct);

        var items = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);

        return (items, totalCount);
    }

    public async Task AddTypePriceAsync(TypePrice typePrice, CancellationToken cancellationToken)
    {
        if (typePrice is null)
            throw new ArgumentNullException(nameof(typePrice));

        await _dbContext.TypePrices.AddAsync(typePrice, cancellationToken);
    }
}