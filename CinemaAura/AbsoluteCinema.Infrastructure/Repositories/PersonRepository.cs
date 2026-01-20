using AbsoluteCinema.Application.Repository;
using AbsoluteCinema.Domain.Entities;
using AbsoluteCinema.Infrastructure.Persistence;

namespace AbsoluteCinema.Infrastructure.Repositories;

public class PersonRepository : BaseRepository<PersonId, Person, CinemaDbContext>, IPersonRepository
{
    public PersonRepository(CinemaDbContext dbContext) : base(dbContext)
    {
    }
}
