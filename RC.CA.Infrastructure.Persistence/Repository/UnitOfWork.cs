using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RC.CA.Application.Contracts.Persistence;

namespace RC.CA.Infrastructure.Persistence.Repository;
/// <summary>
/// Unit of work 
/// </summary>
public class UnitOfWork :IUnitOfWork
{
    private readonly ApplicationDbContext _db;
    public IMemberRepository MemberRepository { get; private set; }
    public IExperienceRepository ExperienceRepository { get; private set; }
    public ICdnFileRepository CdnFileRepository { get; private set; }
    public ICsvFileRepository CsvFileRepository { get; private set; }
    /// <summary>
    /// Construct repository 
    /// </summary>
    /// <param name="db"></param>
    public UnitOfWork(ApplicationDbContext db)
    {
        MemberRepository = new MemberRepository(_db);
        ExperienceRepository = new ExperienceRepository(_db);
        CsvFileRepository = new CsvFileRepository(_db);
        CdnFileRepository = new CdnFileRepository(_db);
        _db = db;
    }
   /// <summary>
   /// Save changes
   /// </summary>
   /// <returns></returns>
    public async Task SaveChangesAsync()
    {
        await _db.SaveChangesAsync();
    }
    
}
