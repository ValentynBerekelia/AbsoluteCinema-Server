
using AbsoluteCinema.Domain.Entities;

namespace AbsoluteCinema.Domain.Specifications
{
    public class MovieWithPersonsSpec : Specification<Movie>
    {
        public MovieWithPersonsSpec(MovieId movieId)
        {
            Criteria = m => m.Id == movieId;
            AddInclude(m => m.Persons);
            ApplyTracking(true);
        }
    }
}
