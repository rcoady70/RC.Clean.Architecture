
using RC.CA.Application.Contracts.Persistence;
using RC.CA.Domain.Entities.Account;
using RC.CA.Domain.Entities.Club;

namespace RC.CA.Infrastructure.Persistence.Repository;
public class JwtRefreshTokenRepository : AsyncRepository<Domain.Entities.Account.JwtRefreshToken>,IJwtRefreshTokenRepository
{
    private readonly ApplicationDbContext _dbContext;
    public JwtRefreshTokenRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }
    /// <summary>
    /// Get most recent jwt refresh token 
    /// </summary>
    /// <param name="User"></param>
    /// <returns></returns>
    public Domain.Entities.Account.JwtRefreshToken GetLatestRefreshTokenByUser(string userId)
    {
        return _dbContext.JwtJwtRefreshTokens.OrderByDescending(j => j.CreatedOn).FirstOrDefault(j => j.UserId == userId);
    }
}
