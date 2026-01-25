using AbsoluteCinema.Application.Repository;
using AbsoluteCinema.Domain.Entities;
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
            .FirstOrDefaultAsync(s => s.Id == id, ct);
    }

    public async Task<List<Session>> GetAllWithDetailsAsync(CancellationToken ct = default)
    {
        return await _set
            .AsNoTracking()
            .Include(s => s.TypePrices)
            .ToListAsync(ct);
    }
}