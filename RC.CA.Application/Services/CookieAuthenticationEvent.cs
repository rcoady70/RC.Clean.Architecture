using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RC.CA.Application.Settings;
using RC.CA.Infrastructure.Logging.Constants;
using RC.CA.SharedKernel.Constants;

namespace RC.CA.Application.Services;
/// <summary>
/// Override cookie authentication to apply a max cookie lifetime. 
/// As sliding cookie is used this prevents the cookie from being re-used indefinitely
/// </summary>
public class CookieAuthenticationEvent : Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationEvents
{
    private const string TicketIssuedTicks = nameof(TicketIssuedTicks);
    private readonly ILogger<CookieAuthenticationEvent> _logger;

    public CookieAuthenticationEvent(ILogger<CookieAuthenticationEvent> logger,
                                            IOptions<JwtSettings> jwtSettings)
    {
        _logger = logger;
    }
    /// <summary>
    /// Store the UTC date-time when the cookie was issued
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    public override async Task SigningIn(CookieSigningInContext context)
    {
        context.Properties.SetString(TicketIssuedTicks,DateTimeOffset.UtcNow.Ticks.ToString());
        await base.SigningIn(context);
    }

    public override async Task ValidatePrincipal(CookieValidatePrincipalContext context)
    {
        //Get date time cookie was set from context properties
        var ticketIssuedTicksValue = context.Properties.GetString(TicketIssuedTicks);
        
        _logger.LogDebug(LoggerEvents.DebugLifeCycle, $"Executing MVC CookieAuthenticationEvent API request {nameof(CookieAuthenticationEvent)} ticketIssuedTicksValue: {ticketIssuedTicksValue}");

        //Convert the ticks from string to int
        if (ticketIssuedTicksValue is null ||!long.TryParse(ticketIssuedTicksValue, out var ticketIssuedTicks))
        {
            _logger.LogWarning(LoggerEvents.SecurityEvtExpiry, @"Cookie expired by middleware CookieAuthenticationEvents max cookie lifespan exceeded CustomCookieAuthenticationEvents {ticketIssuedUtc} user redirected to login", ticketIssuedTicksValue.ToString());
            await RejectPrincipalAsync(context);
            return;
        }

        var ticketIssuedUtc = new DateTimeOffset(ticketIssuedTicks, TimeSpan.FromHours(0));
        var maxTicketUtc = TimeSpan.FromHours(SessionConstants.Session_Cookie_MaxLifeTimeInHours);
        _logger.LogDebug(LoggerEvents.DebugLifeCycle, $"Executing MVC {nameof(CookieAuthenticationEvent)}  mvc request {nameof(CookieAuthenticationEvent)} cookie is {DateTimeOffset.UtcNow - ticketIssuedUtc} old");
        if (DateTimeOffset.UtcNow - ticketIssuedUtc > maxTicketUtc)
        {
            _logger.LogWarning(LoggerEvents.SecurityEvtExpiry, @"Cookie expired by middleware CookieAuthenticationEvents max cookie lifespan exceeded CustomCookieAuthenticationEvents {ticketIssuedUtc} user redirected to login", ticketIssuedUtc.ToString());
            await RejectPrincipalAsync(context);
            return;
        }

        await base.ValidatePrincipal(context);
    }
    /// <summary>
    /// Signout cookie
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    private static async Task RejectPrincipalAsync(CookieValidatePrincipalContext context)
    {
        context.RejectPrincipal();
        //Sign out of default scheme
        //
        await context.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
    }
}
