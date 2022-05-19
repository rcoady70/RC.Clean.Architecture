using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace RC.CA.WebUiMvc.Utilities;

public class WebHelper
{
    public static string GetHeaderValue(HttpContext httpContext,string key,string defaultValue ="")
    {
        if (httpContext == null)
            return defaultValue;
        //Try to get requestId from header. Used to tie error messages between mvc site and api site
        if (!httpContext.Request.Headers.TryGetValue(key, out var value))
            value = defaultValue;
        return value;
    }
    /// <summary>
    /// Get ip address helper
    /// </summary>
    /// <param name="httpContext"></param>
    /// <returns></returns>
    public static string GetIpAddress(HttpContext httpContext)
    {
        if (httpContext == null)
            return "No-IP-Found";
        string? ip = null;
        try
        {
            //Get ip from header first then drop through the different options
            if (httpContext.Request.Headers.ContainsKey("X-Forwarded-For"))
                ip = httpContext.Request.Headers["X-Forwarded-For"].ToString();
            else
            {
                // RemoteIpAddress is always null in DNX RC1 Update1 (bug).
                if (string.IsNullOrEmpty(ip) && httpContext?.Connection?.RemoteIpAddress != null)
                    ip = httpContext.Connection.RemoteIpAddress.ToString();
                else
                {
                    if (string.IsNullOrEmpty(ip))
                    {
                        if (httpContext.Request.Headers.ContainsKey("REMOTE_ADDR"))
                            ip = httpContext.Request.Headers["REMOTE_ADDR"].ToString();
                        else
                            ip = "No-IP-Found";
                    }
                }
            }
        }
        catch
        { }
        return ip;
    }
    /// <summary>
    /// Parse claims from JWT token and create new claims
    /// </summary>
    /// <param name="jwtSecurityToken"></param>
    /// <param name="tokenHandler"></param>
    /// <returns></returns>
    public static async Task<IList<Claim>> AuthorizationParseClaims(string jwtSecurityToken, JwtSecurityTokenHandler tokenHandler)
    {
        JwtSecurityToken tokenContent = tokenHandler.ReadJwtToken(jwtSecurityToken);
        var claims = tokenContent.Claims.ToList();
        claims.Add(new Claim(ClaimTypes.Name, tokenContent.Subject));
        //
        var jwtTokenClaim = new Claim("JwtToken", jwtSecurityToken);
        if (!claims.Contains(jwtTokenClaim))
            claims.Add(jwtTokenClaim);
        return claims;
    }
}
