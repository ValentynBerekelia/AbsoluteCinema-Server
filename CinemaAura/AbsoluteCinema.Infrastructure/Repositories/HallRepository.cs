using AbsoluteCinema.Application.Repository;
using AbsoluteCinema.Domain.Entities;
using AbsoluteCinema.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore; 

namespace AbsoluteCinema.Infrastructure.Repositories;

public class HallRepository : BaseRepository<HallId, Hall, CinemaDbContext>, IHallRepository
{
    public HallRepository(CinemaDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<Hall?> GetByNameAsync(string name, CancellationToken ct = default)
    {
        return await _set
            .FirstOrDefaultAsync(h => h.HallName == name, ct);
    }
}