using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using RC.CA.Application.Dto.Authentication;
using RC.CA.Domain.Entities.Account;
using RC.CA.Infrastructure.Persistence.Identity;

namespace RC.CA.Infrastructure.Persistence.AuthorizationJwt;
/// <summary>
/// Jwt helper function
/// </summary>
public interface IJwtUtilities
{
    Task<string> GenerateJwtTokenAsync(ApplicationUser user, DateTime tokenExpiresAt);
    Task<string> GenerateJwtTokenFromClaimsAsync(IEnumerable<Claim>? claims, DateTime tokenExpiresAt);
    string GenerateRefreshToken();
    Task<JwtRefreshToken> GetJwtRefreshToken(string userName);
    Task<string> RefreshJwtToken(RefreshLoginRequest refreshLoginRequest, DateTime tokenExpiresAt);
    Task RevokeJwtRefreshToken(string jwtRefreshToken, string reason);
    Task SaveJwtRefreshToken(string refreshToken, string userName,string reason);

}
