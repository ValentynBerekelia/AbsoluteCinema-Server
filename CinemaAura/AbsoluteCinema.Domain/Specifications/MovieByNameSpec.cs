using System.Linq.Expressions;
using AbsoluteCinema.Domain.Entities;
using AbsoluteCinema.Domain.Specifications;

namespace AbsoluteCinema.Domain.Specifications;

public class MovieByNameSpec : Specification<Movie>
{
    public MovieByNameSpec(string movieName) : base()
    {
        Criteria = (m => m.Name == movieName);
    }
}