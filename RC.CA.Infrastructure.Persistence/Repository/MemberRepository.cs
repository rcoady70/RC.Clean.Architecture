
using RC.CA.Application.Contracts.Persistence;
using RC.CA.Domain.Entities.Club;

namespace RC.CA.Infrastructure.Persistence.Repository;
public class MemberRepository : AsyncRepository<Member>,IMemberRepository
{
    private readonly ApplicationDbContext _dbContext;
    public MemberRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }

}
