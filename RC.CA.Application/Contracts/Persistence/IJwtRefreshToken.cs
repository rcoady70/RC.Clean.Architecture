using RC.CA.Application.Contracts.Persistence;
using RC.CA.Domain.Entities.Account;

namespace RC.CA.Infrastructure.Persistence.Identity;

public interface IJwtRefreshToken : IAsyncRepository<JwtRefreshToken>
{
    DateTime Created { get; set; }
    string CreatedByIp { get; set; }
    DateTime Expires { get; set; }
    Guid Id { get; set; }
    bool IsActive { get; }
    bool IsExpired { get; }
    bool IsRevoked { get; }
    string ReasonRevoked { get; set; }
    string ReplacedByToken { get; set; }
    DateTime? Revoked { get; set; }
    string RevokedByIp { get; set; }
    string Token { get; set; }
}
