using System.Linq.Expressions;

namespace CinemaAura.Domain.Primitives;

public interface ISpecification<T>
{
    Expression<Func<T, bool>> Criteria { get; }

    IReadOnlyList<Expression<Func<T, object>>> Includes { get; }

    Expression<Func<T, object>>? OrderBy { get; }
    Expression<Func<T, object>>? OrderByDescending { get; }

    int? Skip { get; }
    int? Take { get; }
    bool IsPagingEnabled { get; }

    bool AsNoTracking { get; }
}