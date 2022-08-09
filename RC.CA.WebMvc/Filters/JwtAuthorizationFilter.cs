using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using RC.CA.Application.Contracts.Identity;
using RC.CA.Application.Settings;
using RC.CA.Infrastructure.Logging.Constants;

namespace NT.CA.WebUiMvc.Filters;

/// <summary>
/// Extra validation to check that jwt token has not expired. 
/// When the token is returned from api it is stored in the session cookie, additional validation to verify token is still valid. 
/// It should expire with the cookie.
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class JwtAuthorizationFilter : Attribute, IAsyncAuthorizationFilter
{
    private readonly ILogger<JwtAuthorizationFilter> _logger;
    private readonly IAppContextX _appContextX;
    private readonly JwtSettings _jwtSettings;

    public JwtAuthorizationFilter(ILogger<JwtAuthorizationFilter> logger, IAppContextX appContextX, IOptions<JwtSettings> jwtSettings)
    {
        _logger = logger;
        _appContextX = appContextX;
        _jwtSettings = jwtSettings.Value;
    }
    public Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        _logger.LogDebug(LoggerEvents.DebugLifeCycle, $"Executing authorization filter {nameof(JwtAuthorizationFilter)} Url {Microsoft.AspNetCore.Http.Extensions.UriHelper.GetDisplayUrl(context.HttpContext.Request)}");
        // skip authorization if action is decorated with [AllowAnonymous] attribute
        //
        var allowAnonymous = context.ActionDescriptor.EndpointMetadata.OfType<AllowAnonymousAttribute>().Any();
        bool hasAllowAnonymous = context.ActionDescriptor.EndpointMetadata
                                 .Any(em => em.GetType() == typeof(AllowAnonymousAttribute)); //< -- Here it is
        if (allowAnonymous)
            return Task.CompletedTask;

        if (!string.IsNullOrEmpty(_appContextX.JwtToken))
        {
            var tokenOk = ValidateJwtToken(_jwtSettings.Key, _appContextX.JwtToken, _jwtSettings.Issuer, _jwtSettings.Audience);
            if (!tokenOk) //goto to login page
                context.Result = new ChallengeResult(CookieAuthenticationDefaults.AuthenticationScheme);
        }
        return Task.CompletedTask;
    }

    /// <summary>
    /// Validate JWT token additional validation to ensure the token is still valid
    /// </summary>
    /// <param name="token"></param>
    /// <returns></returns>
    private  bool ValidateJwtToken(string secret, string token,string issuer, string audience)
    {
        bool tokenValid = true;
        if (token == null)
            return false;

        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(secret);
        try
        {
            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidIssuer = issuer,
                ValidAudience = audience,
                ValidateIssuer = true,
                ValidateAudience = true,
                ClockSkew = TimeSpan.Zero
            }, out SecurityToken validatedToken);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(LoggerEvents.SecurityEvtExpiry, @"Security jwt token failed to validate {message}. User redirected to login. ", ex.Message);
            tokenValid = false;
        }
        return tokenValid;
    }
}
