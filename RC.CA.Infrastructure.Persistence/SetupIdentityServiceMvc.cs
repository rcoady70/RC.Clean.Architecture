using System.Text;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.IdentityModel.Tokens;
using RC.CA.Application.Contracts.Identity;
using RC.CA.Application.Dto.Authentication;
using RC.CA.Application.Services;
using RC.CA.Infrastructure.Persistence.AuthorizationJwt;
using RC.CA.Infrastructure.Persistence.Identity;
using RC.CA.Infrastructure.Persistence.Services;

namespace RC.CA.Infrastructure.Persistence;

public static class SetupIdentityServiceMvc
{

    /// <summary>
    /// Setup application context, connection string etc
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    /// <returns></returns>
    public static IServiceCollection SetupIdentityServiceServicesMvc(this IServiceCollection services, IConfiguration configuration)
    {
        //[Authorization] CookieAuthenticationDefaults authorization. Contains the claims from the web api login
        //
        services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
            {
                options.LoginPath = "/Account/UserAccount/Login";
                options.AccessDeniedPath = "/Account/UserAccount/AccessDenied";
                options.ReturnUrlParameter = "returnUrl";
                options.Cookie.Name = RC.CA.SharedKernel.Constants.SessionConstants.Session_Cookie;
                options.Cookie.HttpOnly = true;
                options.Cookie.SameSite = SameSiteMode.Strict;
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                //ExpireTimeSpan defines a lifetime of the authentication ticket. The authentication ticket is a payload of an authentication cookie. 
                options.ExpireTimeSpan = TimeSpan.FromMinutes(RC.CA.SharedKernel.Constants.SessionConstants.Session_Cookie_ExpiryInMin);
                //SlidingExpiration this should only be used with CookieAuthenticationEvents below which set absolute cookie lifetime.  
                options.SlidingExpiration = true; 
                //Sets max age on cookie...
                options.EventsType = typeof(Application.Services.CookieAuthenticationEvent);
                //Cookie lifetime if not set cookie will purge when browser closed. Note that you should not rely solely on Cookie.MaxAge. It can be fully controlled by the user.
                //options.Cookie.MaxAge = options.ExpireTimeSpan; 
            });
        return services;
    }
}
