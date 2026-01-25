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

    // одна сесія з цінами
    public async Task<Session?> GetByIdWithPricesAsync(SessionId id, CancellationToken ct = default)
    {
        return await _set // _set з BaseRepository
            .Include(s => s.TypePrices)
            .FirstOrDefaultAsync(s => s.Id == id, ct);
    }

    public async Task<List<Session>> GetAllWithDetailsAsync(CancellationToken ct = default)
    {
        return await _set
            .AsNoTracking() // оптимізація для читання (швидше)
            .Include(s => s.TypePrices)
            .ToListAsync(ct);
    }
}