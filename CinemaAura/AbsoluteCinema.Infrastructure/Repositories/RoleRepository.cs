using AbsoluteCinema.Application.Repository;
using AbsoluteCinema.Domain.Entities;
using AbsoluteCinema.Infrastructure.Persistence;

namespace AbsoluteCinema.Infrastructure.Repositories;

public class RoleRepository : BaseRepository<RoleId, Role, CinemaDbContext>, IRoleRepository
{
    public RoleRepository(CinemaDbContext dbContext) : base(dbContext)
    {
    }
}
