using AbsoluteCinema.Application.Repository;
using AbsoluteCinema.Domain.Entities;
using AbsoluteCinema.Infrastructure.Persistence;

namespace AbsoluteCinema.Infrastructure.Repositories;

public class GenreRepository : BaseRepository<GenreId, Genre, CinemaDbContext>, IGenreRepository
{
    public GenreRepository(CinemaDbContext dbContext) : base(dbContext)
    {
    }
}
