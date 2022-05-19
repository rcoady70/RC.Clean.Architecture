using Microsoft.EntityFrameworkCore;
using RC.CA.Application.Contracts.Persistence;
using RC.CA.Application.Dto;
using RC.CA.Application.Extensions.Linq;

namespace RC.CA.Infrastructure.Persistence.Repository;



/// <summary>
/// Paged list generic type. Generate a pages list of type T
/// </summary>
/// <typeparam name="T"></typeparam>
public class PagedList<T> : List<T>, IPagedList
{
    public PaginationMetaData PagnationMetaData { get; set; } = new PaginationMetaData();
    /// <summary>
    /// Construct paged list of items
    /// </summary>
    /// <param name="items"></param>
    /// <param name="count"></param>
    /// <param name="pageNumber"></param>
    /// <param name="pageSize"></param>
    public PagedList(List<T> items, int count, int pageNumber, int itemsPerPage)
    {
        PagnationMetaData.TotalItems = count;
        PagnationMetaData.ItemsPerPage = itemsPerPage;
        PagnationMetaData.CurrentPage = pageNumber;
        PagnationMetaData.TotalPages = (int)Math.Ceiling(count / (double)itemsPerPage);
        AddRange(items);
    }
    /// <summary>
    /// Create paged list 
    /// </summary>
    /// <param name="source">IQueryable<T> query</param>
    /// <param name="orderByCollection">Order by collection</param>
    /// <param name="pageNumber">page number</param>
    /// <param name="pageSize">Page size</param>
    /// <returns></returns>
    public static async Task<PagedList<T>> CreateAsync(IQueryable<T> query, List<SortModel>? orderByCollection,
                                                      int pageNumber, int pageSize)
    {
        pageNumber = pageNumber == 0 ? 1 : pageNumber;
        pageSize = pageSize < 1 ? 10 : pageSize;
        pageSize = pageSize > 250 ? 250 : pageSize;

        List<T> items;
        var count = query.Count();
        if (orderByCollection != null)
            items = await query.OrderByCollectionExt(orderByCollection)
                                .Skip((pageNumber - 1) * pageSize)
                                .Take(pageSize)
                                .ToListAsync();
        else
            items = await query.Skip((pageNumber - 1) * pageSize)
                               .Take(pageSize)
                               .ToListAsync();

        return new PagedList<T>(items, count, pageNumber, pageSize);
    }
}
