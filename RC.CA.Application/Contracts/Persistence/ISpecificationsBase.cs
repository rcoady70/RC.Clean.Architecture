using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace RC.CA.Application.Contracts.Persistence;
/// <summary>
/// Repository specifications
/// </summary>
public interface ISpecificationBase<T>
{
    // Filter Conditions
    Expression<Func<T, bool>> FilterCondition { get; }

    // Order By Ascending
    Expression<Func<T, object>> OrderBy { get; }

    // Order By Descending
    Expression<Func<T, object>> OrderByDescending { get; }

    // Include collection to load related data
    List<Expression<Func<T, object>>> Includes { get; }

    // GroupBy expression
    Expression<Func<T, object>> GroupBy { get; }
}
