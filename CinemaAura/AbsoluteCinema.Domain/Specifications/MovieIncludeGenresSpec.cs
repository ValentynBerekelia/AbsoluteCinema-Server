using AbsoluteCinema.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbsoluteCinema.Domain.Specifications
{
    public class MovieIncludeGenresSpec : Specification<Movie>
    {
        public MovieIncludeGenresSpec(MovieId movieId)
        {
            Criteria = m => m.Id == movieId;

            // include medias
            AddInclude(m=>m.Genres);

            ApplyTracking(true);
        }
    }
}
