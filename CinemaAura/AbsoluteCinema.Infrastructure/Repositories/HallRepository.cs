using AbsoluteCinema.Application.Repository;
using AbsoluteCinema.Domain.Entities;
using AbsoluteCinema.Infrastructure.Persistence;

namespace AbsoluteCinema.Infrastructure.Repositories;

public class HallRepository : BaseRepository<HallId, Hall, CinemaDbContext>, IHallRepository
{
    public HallRepository(CinemaDbContext dbContext) : base(dbContext)
    {
    }
}
