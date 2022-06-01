using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using RC.CA.WebUiMvc.Services;
using RC.CA.Application.Contracts.Identity;
using RC.CA.Application.Models;
using RC.CA.Application.Dto.Authentication;
using RC.CA.WebUiMvc.Utilities;
using Microsoft.AspNetCore.Authorization;
using RC.CA.Application.Contracts.Services;

namespace RC.CA.WebUiMvc.Areas.Account.Controllers;
[Area("Account")]
[AllowAnonymous]
public class UserAccountController : RootController
{
    private readonly IHttpHelper _httpHelper;

    [BindProperty]
    public string? ReturnUrl { get; set; }
    
    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="httpClientFactory"></param>
    
    public UserAccountController(IAppContextX appContext, IHttpHelper httpHelper) :base(appContext)
    {
        _httpHelper = httpHelper;
    }
    /// <summary>
    /// Log out session
    /// </summary>
    /// <param name="loginRequest"></param>
    /// <returns></returns>
    [HttpPost]
    
    public async Task<IActionResult> LogOut()
    {
        var authResponse = await _httpHelper.SendAsync<EmptyRequest, LoginResponse>(new EmptyRequest(), "api/UserAccount/Logout", HttpMethod.Post);
        if (authResponse.TotalErrors == 0)
        {
            //Logout local cookie
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            if (authResponse?.TotalErrors == 0)
                return RedirectToAction("index", "home", new { area = "home" });
        }
        return View();
    }
    /// <summary>
    /// Login
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<IActionResult> Login(string returnUrl)
    {
        var loginRequest = new LoginRequest()
        {
            ReturnUrlX = returnUrl
        };
        return View(loginRequest);
    }
    /// <summary>
    /// Login post action
    /// </summary>
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> Login(LoginRequest loginRequest)
    {
        if (!ModelState.IsValid) return View();

        var authResponse = await _httpHelper.SendAsync<LoginRequest, LoginResponse>(loginRequest, "api/UserAccount/Login", HttpMethod.Post);
        if (authResponse?.TotalErrors == 0)
        {
            //Parse and generate new claims object
            var claims = await WebHelper.AuthorizationParseClaims(authResponse.AccessToken,new JwtSecurityTokenHandler());
            var identity = new ClaimsPrincipal(new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme));

            var authProperties = new AuthenticationProperties
            {
                IsPersistent = loginRequest.RememberMe,
            };

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, identity, authProperties);
            bool m = User.Identity.IsAuthenticated;
            if (string.IsNullOrEmpty(loginRequest.ReturnUrlX) || loginRequest.ReturnUrlX.Contains("/Login"))
                return RedirectToAction("index", "home", new { area = "home" });
            else
                return LocalRedirect(loginRequest.ReturnUrlX);
        }
        await AppendErrorsToModelStateAsync(authResponse);
        return View();
    }

    [HttpGet]
    public async Task<IActionResult> Register()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Register(RegistrationRequest registrationRequest)
    {
        if (!ModelState.IsValid) return View();
        //
        var authResponse = await _httpHelper.SendAsync<RegistrationRequest, RegistrationResponse>(registrationRequest, "api/UserAccount/Register", HttpMethod.Post);
        if (authResponse?.TotalErrors == 0)
            return RedirectToAction("index", "home", new { area = "home" });
        await AppendErrorsToModelStateAsync(authResponse);
        return View();
    }
}
