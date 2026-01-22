using AbsoluteCinema.Application.Repository;
using AbsoluteCinema.Domain.Entities;
using AbsoluteCinema.Infrastructure.Persistence;

namespace AbsoluteCinema.Infrastructure.Repositories;

public class MediaRepository : BaseRepository<MediaId, Media, CinemaDbContext>, IMediaRepository
{
    public MediaRepository(CinemaDbContext dbContext) : base(dbContext)
    {
    }
}
