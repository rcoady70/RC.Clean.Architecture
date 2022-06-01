using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RC.CA.Application.Contracts.Identity;
using RC.CA.Domain.Entities.Account;
using RC.CA.Domain.Entities.Cdn;
using RC.CA.Domain.Entities.Club;
using RC.CA.Domain.Entities.CSV;
using RC.CA.Infrastructure.Persistence.EntityConfig;
using RC.CA.Infrastructure.Persistence.Repository;

namespace RC.CA.Infrastructure.Persistence;

public class ApplicationDbContext : IdentityDbContext
{
    private readonly IAppContextX _appContextX;
    public DbSet<CdnFiles> CdnFiles { get; set; }
    public DbSet<CsvFile> CsvFile { get; set; }
    public DbSet<Member> Members { get; set; }
    public DbSet<JwtRefreshToken> JwtJwtRefreshTokens { get; set; }
    public IAppContextX AppContextX
    {
        get { return _appContextX; }
    }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IAppContextX appContextX) : base(options)
    {
        //Contains user context, hydrated claims, user name, ip address etc
        _appContextX = appContextX;
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        //
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
    
}
