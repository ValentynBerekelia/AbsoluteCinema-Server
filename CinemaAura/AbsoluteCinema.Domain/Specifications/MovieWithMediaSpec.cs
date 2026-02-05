using AbsoluteCinema.Domain.Entities;

namespace AbsoluteCinema.Domain.Specifications
{
    /// <summary>
    /// Specification for get Movie with Medias collection (for update)
    /// </summary>
    public class MovieWithMediasSpec : Specification<Movie>
    {
        public MovieWithMediasSpec(MovieId movieId)
        {
            Criteria = m => m.Id == movieId;

            // include medias
            AddInclude(m => m.Medias);

            ApplyTracking(true);
        }
    }
}
