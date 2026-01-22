using AbsoluteCinema.Application.Repository;
using AbsoluteCinema.Domain.Entities;
using AbsoluteCinema.Infrastructure.Persistence;

namespace AbsoluteCinema.Infrastructure.Repositories;

public class PermissionRepository : BaseRepository<PermissionId, Permission, CinemaDbContext>, IPermissionRepository
{
    public PermissionRepository(CinemaDbContext dbContext) : base(dbContext)
    {
    }
}
