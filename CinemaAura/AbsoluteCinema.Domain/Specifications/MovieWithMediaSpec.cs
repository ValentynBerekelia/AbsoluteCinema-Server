using AbsoluteCinema.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
