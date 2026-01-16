using System.Linq.Expressions;
using CinemaAura.Domain.Primitives;

namespace CinemaAura.Domain.Specifications;

public abstract class Specification<T> : ISpecification<T>
{
    protected Specification(Expression<Func<T, bool>> criteria)
        => Criteria = criteria;

    public Expression<Func<T, bool>> Criteria { get; }

    private readonly List<Expression<Func<T, object>>> _includes = new();
    public IReadOnlyList<Expression<Func<T, object>>> Includes => _includes;

    public Expression<Func<T, object>>? OrderBy { get; private set; }
    public Expression<Func<T, object>>? OrderByDescending { get; private set; }

    public int? Skip { get; private set; }
    public int? Take { get; private set; }
    public bool IsPagingEnabled { get; private set; }

    public bool AsNoTracking { get; private set; } = true;

    protected void AddInclude(Expression<Func<T, object>> include) => _includes.Add(include);

    protected void ApplyOrderBy(Expression<Func<T, object>> orderBy) => OrderBy = orderBy;
    protected void ApplyOrderByDescending(Expression<Func<T, object>> orderByDesc) => OrderByDescending = orderByDesc;

    protected void ApplyPaging(int skip, int take)
    {
        Skip = skip;
        Take = take;
        IsPagingEnabled = true;
    }

    protected void ApplyTracking(bool tracking) => AsNoTracking = !tracking;
}