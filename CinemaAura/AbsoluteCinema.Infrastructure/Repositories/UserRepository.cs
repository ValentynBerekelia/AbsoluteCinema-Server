using AbsoluteCinema.Application.Repository;
using AbsoluteCinema.Domain.Entities;
using AbsoluteCinema.Infrastructure.Persistence;

namespace AbsoluteCinema.Infrastructure.Repositories;

public class UserRepository : BaseRepository<UserId, User, CinemaDbContext>, IUserRepository
{
    public UserRepository(CinemaDbContext dbContext) : base(dbContext)
    {
    }
}
