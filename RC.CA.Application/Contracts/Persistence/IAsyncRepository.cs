using System.Linq.Expressions;
using RC.CA.Application.Extensions.Linq;

namespace RC.CA.Application.Contracts.Persistence;

public interface IAsyncRepository<TEntity> where TEntity : class
{
    Task<TEntity> GetFirstOrDefault(Expression<Func<TEntity, bool>> filter, string? includeProperties = null, bool tracked = false);
    Task<IReadOnlyList<TEntity>> GetAll(Expression<Func<TEntity, bool>>? filter = null, string? includeProperties = null);
    Task<int> GetCountAsync(Expression<Func<TEntity, bool>>? filter,int take);
    Task<bool> ExistsAsync(Guid? id);
    Task<bool> ExistsAsync(int id);
    Task<bool> ExistsAsync(string id);
    Task<IReadOnlyList<TEntity>> ListAllAsync();
    Task<TEntity> AddAsync(TEntity entity);
    Task<int> UpdateAsync(TEntity entity);
    Task DeleteAsync(TEntity entity);
    Task DeleteRangeAsync(IEnumerable<TEntity> entity);
    Task<IPagedList> GetPagedListAsync(int pageIndex, int pageSize, Expression<Func<TEntity, bool>>? filter, List<SortModel>? orderByCollection, string includeProperties);
    Task SaveChangesAsync();
}
