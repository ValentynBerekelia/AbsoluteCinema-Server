using System.Linq.Expressions;
using AbsoluteCinema.Domain.Primitives;

namespace AbsoluteCinema.Domain.Specifications;

public abstract class Specification<T> : ISpecification<T>
{
    protected Specification(Expression<Func<T, bool>> criteria)
        => Criteria = criteria;
    protected Specification(){}

    public Expression<Func<T, bool>> Criteria { get; set; }

    private readonly List<Expression<Func<T, object>>> _includes = new();
    public IReadOnlyList<Expression<Func<T, object>>> Includes => _includes;

    private readonly List<string> _includeStrings = new();

    

    public IReadOnlyList<string> IncludeStrings => _includeStrings;

    public Expression<Func<T, object>>? OrderBy { get; set; }
    public Expression<Func<T, object>>? OrderByDescending { get; set; }

    public int? Skip { get; private set; }
    public int? Take { get; private set; }
    public bool IsPagingEnabled { get; private set; }

    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public bool? IsSplitQuery { get; set; }

    public bool AsNoTracking { get; set; } = true;

    protected void AddInclude(Expression<Func<T, object>> include) => _includes.Add(include);
    protected void AddIncludeString(string include) => _includeStrings.Add(include);

    protected void ApplyOrderBy(Expression<Func<T, object>> orderBy) => OrderBy = orderBy;
    protected void ApplyOrderByDescending(Expression<Func<T, object>> orderByDesc) => OrderByDescending = orderByDesc;

    protected void ApplyPaging(int pageNumber, int pageSize)
    {
        PageNumber = pageNumber;
        PageSize = pageSize;
    }

    protected void ApplySplitQuery() => IsSplitQuery = true;

    protected void ApplyTracking(bool tracking) => AsNoTracking = !tracking;
}