using AbsoluteCinema.Application.Repository;
using AbsoluteCinema.Domain.Entities;
using AbsoluteCinema.Infrastructure.Persistence;

namespace AbsoluteCinema.Infrastructure.Repositories;

public class SeatRepository : BaseRepository<SeatId, Seat, CinemaDbContext>, ISeatRepository
{
    public SeatRepository(CinemaDbContext dbContext) : base(dbContext)
    {
    }
}
