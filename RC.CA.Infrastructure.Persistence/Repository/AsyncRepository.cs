using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using RC.CA.Application.Contracts.Persistence;
using RC.CA.Application.Extensions.Linq;
using RC.CA.Domain.Entities.Shared;

namespace RC.CA.Infrastructure.Persistence.Repository;

public class AsyncRepository<T> : IAsyncRepository<T> where T : class
{
    protected readonly ApplicationDbContext _dbContext;
    internal DbSet<T> _dbSet;

    public AsyncRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
        _dbSet = _dbContext.Set<T>();
        _dbContext.ChangeTracker.StateChanged += ChangeTracker_StateChanged;
        _dbContext.ChangeTracker.Tracked += ChangeTracker_Tracked;
    }
    /// <summary>
    /// Debug tool triggered if entity is tracked
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void ChangeTracker_Tracked(object sender, EntityTrackedEventArgs e)
    {

        //https://docs.microsoft.com/en-us/shows/visual-studio-toolbox/entity-framework-core-in-depth-part-9
        var source = (e.FromQuery) ? "Database" : "Code";
        Console.WriteLine($"EF debug: Tracked {e.Entry.Entity.GetType().Name}  Source: {source}");
    }
    /// <summary>
    /// Debug change tracker triggered when state changes
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void ChangeTracker_StateChanged(object sender, EntityStateChangedEventArgs e)
    {
        var action = string.Empty;
        Console.WriteLine($"EF debug: state change {e.Entry.Entity.GetType().Name} was {e.OldState} before the state changed to {e.NewState}");
    }
    /// <summary>
    /// Debug took enables you to see change tracker details
    /// </summary>
    /// <returns></returns>
    public string DebugShortView()
    {
        _dbContext.ChangeTracker.DetectChanges();
        return $"EF debug - changes: {_dbContext.ChangeTracker.ToDebugString()}";
    }
    /// <summary>
    /// Get first or default
    /// </summary>
    /// <param name="filter"></param>
    /// <param name="includeProperties"></param>
    /// <param name="tracked"></param>
    /// <returns></returns>
    public virtual async Task<T> GetFirstOrDefaultAsync(Expression<Func<T, bool>> filter, string? includeProperties = null, bool tracked = false)
    {
        IQueryable<T> query = _dbSet;
        if (tracked)
            query = _dbSet.AsTracking();

        query = query.Where(filter);

        //Set include tables
        if (includeProperties != null)
        {
            foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                query = query.Include(includeProperty);
        }

        var result = await query.FirstOrDefaultAsync();
        return result;
    }
    /// <summary>
    /// Get all async
    /// </summary>
    /// <param name="filter"></param>
    /// <param name="includeProperties"></param>
    /// <returns></returns>
    public async Task<IReadOnlyList<T>> GetAll(Expression<Func<T, bool>>? filter = null, string? includeProperties = null)
    {
        IQueryable<T> query = _dbSet;
        if (filter != null)
            query = query.Where(filter);
        if (includeProperties != null)
        {
            foreach (var includeProp in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                query = query.Include(includeProp);
        }
        return await query.ToListAsync();
    }
    /// <summary>
    /// Count records
    /// </summary>
    /// <param name="filter">Filter parameters</param>
    /// <param name="take">Option to set upper limit to ensure query finishes in a timely manor. 0 Count all. For paginated lists set upper value</param>
    /// <returns></returns>
    public virtual async Task<int> GetCountAsync(Expression<Func<T, bool>> filter = null, int take = 0)
    {
        IQueryable<T> query = _dbContext.Set<T>();
        if (filter != null)
            query = query.Where(filter);
        if (take > 0)
            query = query.Take(take);
        var count = await query.CountAsync();
        return count;
    }
    /// <summary>
    /// Find may be preferable as it caches the record
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <param name="id"></param>
    /// <param name=""></param>
    /// <param name="tracked"></param>
    /// <returns></returns>
    public async Task<T> FindAsync<TKey>(TKey id, bool tracked = false)
    {
        if (tracked)
            _dbContext.Set<T>().AsTracking();
        var entity = await _dbContext.Set<T>().FindAsync(id);
        return entity;
    }
    /// <summary>
    /// Find may be preferable as it caches the record
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<bool> ExistsAsync<TKey>(TKey id)
    {
        if (id == null)
            return false;
        var entity = await _dbContext.Set<T>().FindAsync(id);
        return entity != null;
    }

    public async Task<IReadOnlyList<T>> ListAllAsync()
    {
        return await _dbContext.Set<T>().ToListAsync();
    }

    public async Task<T> AddAsync(T entity)
    {
        try
        {
            await AuditBaseEntity();
            await _dbSet.AddAsync(entity);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            throw ex;
        }
        return entity;
    }
    /// <summary>
    /// Get list of tracked entities
    /// </summary>
    /// <returns></returns>
    public IEnumerable<EntityEntry> GetTrackedEntities()
    {
        return _dbContext.ChangeTracker.Entries().ToList();
    }
    public async Task<int> UpdateAsync(T entity)
    {
        int rowCount = 0;
        try
        {
            await AuditBaseEntity();
            _dbSet.Update(entity);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            throw ex;
        }
        return rowCount;
    }
    public async Task DeleteRangeAsync(IEnumerable<T> entity)
    {
        _dbContext.Set<T>().RemoveRange(entity);
    }
    public async Task DeleteAsync(T entity)
    {
        try
        {
            _dbContext.Set<T>().Remove(entity);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            throw ex;
        }
    }
    /// <summary>
    /// Save changes
    /// </summary>
    /// <returns></returns>
    public async Task SaveChangesAsync()
    {
        try
        {
            string m = DebugShortView();
            await _dbContext.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException ex)
        {
            Console.WriteLine(ex.Message);
            EntityEntry entryEntity = ex.Entries[0];
            //Kept in DbChangeTracker
            PropertyValues originalValues = entryEntity.OriginalValues;
            PropertyValues currentValues = entryEntity.CurrentValues;
            IEnumerable<PropertyEntry> modifiedEntries =
                entryEntity.Properties.Where(e => e.IsModified);
            foreach (var itm in modifiedEntries)
            {
                Console.WriteLine($"{itm.Metadata.Name},");
            }
            //Needs to call to database to get values
            PropertyValues databaseValues = entryEntity.GetDatabaseValues();
            //Discards local changes, gets database values, resets change tracker
            entryEntity.Reload();
            //logging stuff here
            throw;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            throw ex;
        }
    }
    /// <summary>
    /// Audit base entities, add create by and updated by information
    /// </summary>
    /// <returns></returns>
    private async Task AuditBaseEntity()
    {
        foreach (var entry in _dbContext.ChangeTracker.Entries<IBaseEntity>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedBy = _dbContext.AppContextX.UserName;
                    entry.Entity.CreatedOn = DateTime.Now;
                    break;
                case EntityState.Modified:
                    entry.Entity.UpdatedBy = _dbContext.AppContextX.UserName;
                    entry.Entity.UpdatedOn = DateTime.Now;
                    break;
            }
        }
    }
    /// <summary>
    /// Get paged list 
    /// </summary>
    /// <param name="pageIndex"></param>
    /// <param name="pageSize"></param>
    /// <param name="filter"></param>
    /// <param name="orderByCollection"></param>
    /// <param name="includeProperties"></param>
    /// <returns></returns>
    public async Task<IPagedList> GetPagedListAsync(int pageIndex, int pageSize,
                                                    Expression<Func<T, bool>>? filter, List<SortModel>? orderByCollection,
                                                    string includeProperties)
    {
        IQueryable<T> query = _dbContext.Set<T>();

        //Set filter
        if (filter != null)
            query = query.Where(filter);

        //Set include tables
        foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            query = query.Include(includeProperty);

        //If order by collection set query using extension method
        return await PagedList<T>.CreateAsync(query, orderByCollection, pageIndex, pageSize);
    }
}

