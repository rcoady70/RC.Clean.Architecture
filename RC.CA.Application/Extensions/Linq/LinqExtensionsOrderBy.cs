using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RC.CA.Application.Extensions.Linq;
public static class LinqExtensionsOrderBy
{
    /// <summary>
    /// Order by a collection of field names
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="source"></param>
    /// <param name="sortModels"></param>
    /// <returns></returns>
    public static IQueryable<T> OrderByCollectionExt<T>(this IQueryable<T> source, IEnumerable<SortModel> sortCollection)
    {
        string colName = "";
        try
        {
            var expression = source.Expression;
            int count = 0;
            foreach (var item in sortCollection)
            {
                colName = item.ColName;
                var parameter = Expression.Parameter(typeof(T), "x");
                var selector = Expression.PropertyOrField(parameter, colName);
                var method = string.Equals(item.Sort.ToString(), "desc", StringComparison.OrdinalIgnoreCase) ?
                                                      (count == 0 ? "OrderByDescending" : "ThenByDescending") :
                                                      (count == 0 ? "OrderBy" : "ThenBy");
                expression = Expression.Call(typeof(Queryable),
                                             method,
                                             new Type[] { source.ElementType, selector.Type },
                                             expression,
                                             Expression.Quote(Expression.Lambda(selector, parameter)));
                count++;
            }

            return count > 0 ? source.Provider.CreateQuery<T>(expression) : source;
        }
        catch
        {
            throw new ArgumentException($"OrderByCollectionExt failed with column name {colName} entity {source.ElementType.Name} ",colName);
        }
    }
}

public class SortModel
{
    public enum SortType
    {
        asc,
        desc,
    }
    public SortModel()
    {
    }
    /// <summary>
    /// Construct from col name. List processing passes order with col name example Name_asc Gender_desc
    /// </summary>
    /// <param name="colName">Col name</param>
    public SortModel(string colName)
    {
        var colValues = colName.Split('_');
        if (colValues.Length>0)
            this.ColName = colName.Split('_')[0];
        if (colValues.Length > 1)
            this.Sort = Enum.TryParse(colName.Split('_')[1], out SortType sType) ? sType : SortType.asc;
    }
    public SortModel(string colName, SortType sortOrder)
    {
        
        this.ColName = colName;
        this.Sort = sortOrder;
    }
    public string ColName { get; set; } = "";
    public SortType Sort { get; set; } =  SortType.asc;
    public string PairAsSqlExpression
    {
        get
        {
            return $"{ColName} {Sort}";
        }
    }
}
