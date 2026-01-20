using AbsoluteCinema.Application.Repository;
using AbsoluteCinema.Domain.Entities;
using AbsoluteCinema.Infrastructure.Persistence;

namespace AbsoluteCinema.Infrastructure.Repositories;

public class MovieRepository : BaseRepository<MovieId, Movie, CinemaDbContext>, IMovieRepository
{
    public MovieRepository(CinemaDbContext dbContext) : base(dbContext)
    {
    }
}
