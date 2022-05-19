using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using RC.CA.Application.Contracts.Persistence;

namespace RC.CA.Infrastructure.Persistence.Specifications;
public class SpecificationBase<T> : ISpecificationBase<T>
{
    private readonly List<Expression<Func<T, object>>> _includeCollection = new List<Expression<Func<T, object>>>();

    public SpecificationBase()
    {
    }

    public SpecificationBase(Expression<Func<T, bool>> filterCondition)
    {
        this.FilterCondition = filterCondition;
    }

    public bool IsSatisfiedBy(T entity)
    {
        Func<T, bool> predicate = this.FilterCondition.Compile();
        return predicate(entity);
    }

    public SpecificationBase<T> And(SpecificationBase<T> specification)
    {
        return new AndSpecification<T>(this, specification);
    }

    public SpecificationBase<T> Or(SpecificationBase<T> specification)
    {
        return new OrSpecification<T>(this, specification);
    }

    public virtual Expression<Func<T, bool>> FilterCondition { get; private set; }
    public Expression<Func<T, object>> OrderBy { get; private set; }
    public Expression<Func<T, object>> OrderByDescending { get; private set; }
    public List<Expression<Func<T, object>>> Includes
    {
        get
        {
            return _includeCollection;
        }
    }

    public Expression<Func<T, object>> GroupBy { get; private set; }

    protected void AddInclude(Expression<Func<T, object>> includeExpression)
    {
        Includes.Add(includeExpression);
    }

    protected void ApplyOrderBy(Expression<Func<T, object>> orderByExpression)
    {
        OrderBy = orderByExpression;
    }

    protected void ApplyOrderByDescending(Expression<Func<T, object>> orderByDescendingExpression)
    {
        OrderByDescending = orderByDescendingExpression;
    }

    protected void SetFilterCondition(Expression<Func<T, bool>> filterExpression)
    {
        FilterCondition = filterExpression;
    }

    protected void ApplyGroupBy(Expression<Func<T, object>> groupByExpression)
    {
        GroupBy = groupByExpression;
    }
}
