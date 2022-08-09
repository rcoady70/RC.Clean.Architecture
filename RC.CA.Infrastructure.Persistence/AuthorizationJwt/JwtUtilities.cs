using System.IdentityModel.Tokens.Jwt;
using System.Security;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using RC.CA.Application.Contracts.Identity;
using RC.CA.Application.Contracts.Persistence;
using RC.CA.Application.Dto.Authentication;
using RC.CA.Application.Settings;
using RC.CA.Domain.Entities.Account;
using RC.CA.Infrastructure.Persistence.Identity;
using RC.CA.SharedKernel.Constants;
using RC.CA.SharedKernel.Extensions;
using RC.CA.SharedKernel.GuardClauses;

namespace RC.CA.Infrastructure.Persistence.AuthorizationJwt;

public class JwtUtilities : IJwtUtilities
{
    private readonly JwtSettings _jwtSettings;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IJwtRefreshTokenRepository _jwtRefreshTokenRepo;
    private readonly IAppContextX _appContextX;

    public JwtUtilities(IOptions<JwtSettings> jwtSettings,
                        UserManager<ApplicationUser> userManager,
                        IJwtRefreshTokenRepository jwtRefreshTokenRepo,
                        IAppContextX appContextX)
    {
        _jwtSettings = jwtSettings.Value;
        _userManager = userManager;
        _jwtRefreshTokenRepo = jwtRefreshTokenRepo;
        _appContextX = appContextX;
    }

    /// <summary>
    /// Generate refresh token string
    /// </summary>
    /// <returns></returns>
    public string GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
    }
    /// <summary>
    /// Generate new jwt token using refresh token
    /// </summary>
    /// <param name="ipAddress"></param>
    /// <returns></returns>
    public async Task<string> RefreshJwtToken(RefreshLoginRequest refreshLoginRequest, DateTime tokenExpiresAt)
    {
        //Read existing token and validate it
        var tokenHandler = new JwtSecurityTokenHandler();
        var principle = tokenHandler.ValidateToken(refreshLoginRequest.JwtToken, new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(_jwtSettings.Key.ToByteArrayExt()),
            ValidIssuer = _jwtSettings.Issuer,
            ValidAudience = _jwtSettings.Audience,
            ValidateIssuer = true,
            ValidateAudience = true,
            ClockSkew = TimeSpan.Zero,
        }, out SecurityToken validatedToken);

        //Check token is not null and is signed we do not allow unsigned tokens
        var jwtToken = validatedToken as JwtSecurityToken;
        if (jwtToken == null || !jwtToken.SignatureAlgorithm.Equals("HS256"))
            throw new SecurityException("Not possible to refresh jwt token as it was not valid");
       
        //Get refresh token from database to ensure it is the currently active refresh token
        var dbRefreshToken = await GetJwtRefreshToken(principle.Identity.Name);
        if (refreshLoginRequest.RefreshToken != dbRefreshToken.Token)
            throw new SecurityException("Not possible to refresh jwt token. Refresh token does not match current refresh token");
        if (dbRefreshToken == null)
            throw new SecurityException("Not possible to refresh jwt token. Refresh token could not be found");
        if (dbRefreshToken.IsExpired)
            throw new SecurityException("Not possible to refresh jwt token. Refresh token expired");
        if (dbRefreshToken.IsRevoked)
            throw new SecurityException("Not possible to refresh jwt token. Refresh token revoked expired");
        //Revoke existing refresh token
        RevokeJwtRefreshToken(dbRefreshToken.Token,"Refresh request");

        //Generate new jwt token from existing claims 
        var jwtSecurityToken = await GenerateJwtTokenFromClaimsAsync(principle.Claims, tokenExpiresAt);
        
        return jwtSecurityToken;
    }
    /// <summary>
    /// Generate jwt token from list of claims
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    public async Task<string> GenerateJwtTokenFromClaimsAsync(IEnumerable<Claim>? claims, DateTime tokenExpiresAt)
    {
        var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
        var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

        var jwtSecurityToken = new JwtSecurityToken(
                                    issuer: _jwtSettings.Issuer,
                                    audience: _jwtSettings.Audience,
                                    notBefore: DateTime.UtcNow,
                                    claims: claims,
                                    expires: tokenExpiresAt,
                                    signingCredentials: signingCredentials);
        var securityToken = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
        return securityToken;
    }
    /// <summary>
    /// Generate jwt token
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    public async Task<string> GenerateJwtTokenAsync(ApplicationUser user, DateTime tokenExpiresAt)
    {
        var userClaims = await _userManager.GetClaimsAsync(user);
        var roles = await _userManager.GetRolesAsync(user);

        var roleClaims = new List<Claim>();

        for (int i = 0; i < roles.Count; i++)
            roleClaims.Add(new Claim("roles", roles[i]));

        var claims = new[]
        {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()), //uniqueidentifier
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(SessionConstants.ClaimUserId, user.Id.ToString())
        }
        .Union(userClaims)
        .Union(roleClaims);

        var jwtSecurityToken = await GenerateJwtTokenFromClaimsAsync(claims, tokenExpiresAt);

        return jwtSecurityToken;
    }
    /// <summary>
    /// Save jwt refresh token to database
    /// </summary>
    /// <param name="refreshToken"></param>
    /// <param name="userName"></param>
    /// <returns></returns>
    public async Task SaveJwtRefreshToken(string refreshToken, string userName,string reason)
    {
        var jwtRefreshToken = new JwtRefreshToken()
        {
            Id = Guid.NewGuid(),
            UserId = userName,
            Token = refreshToken,
            ExpiresOn = DateTime.UtcNow.AddDays(SessionConstants.Session_Jwt_RefreshTokenInHours),
            CreatedOn = DateTime.UtcNow,
            CreatedByIp = _appContextX.IpAddress,
            CreatedBy = userName
        };

        //Revoke old refresh token
        //
        var dToken = await _jwtRefreshTokenRepo.GetFirstOrDefaultAsync(j => j.UserId == userName, null, true);
        if (dToken != null)
        {
           dToken.RevokedOn = DateTime.UtcNow;
           dToken.RevokedByIp = _appContextX.IpAddress;
           dToken.ReasonRevoked = reason;
           await _jwtRefreshTokenRepo.UpdateAsync(dToken);
        }
        await _jwtRefreshTokenRepo.AddAsync(jwtRefreshToken);
        await _jwtRefreshTokenRepo.SaveChangesAsync();
        return;
    }
    /// <summary>
    /// Get jwt refresh token from database
    /// </summary>
    /// <param name="refreshToken"></param>
    /// <param name="userName"></param>
    /// <returns></returns>
    public async Task<JwtRefreshToken> GetJwtRefreshToken(string userName)
    {
        var rToken = _jwtRefreshTokenRepo.GetLatestRefreshTokenByUser(userName);
        return rToken;
    }
    /// <summary>
    /// Revoke refresh token 
    /// </summary>
    /// <param name="refreshToken"></param>
    /// <param name="userName"></param>
    /// <returns></returns>
    public async Task RevokeJwtRefreshToken(string userId, string reason)
    {
        Guard.Against.NullOrEmpty(userId, nameof(userId));
        var rToken = await _jwtRefreshTokenRepo.GetFirstOrDefaultAsync(j => j.UserId == userId && j.RevokedOn == null, null, true);
        if (rToken != null)
        {
            rToken.RevokedOn = DateTime.UtcNow;
            rToken.RevokedByIp = _appContextX.IpAddress;
            rToken.ReasonRevoked = reason;
            await _jwtRefreshTokenRepo.UpdateAsync(rToken);
        }
        await _jwtRefreshTokenRepo.SaveChangesAsync();
        return;
    }
}
