using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using RC.CA.Application.Contracts.Identity;
using RC.CA.Application.Dto.Authentication;

namespace RC.CA.WebApi.Areas.Account.Controllers;
[Route("api/[controller]")]
[ApiController]
public class UserAccountController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IAuthService _authenticationService;
    private readonly JwtSettings _jwtSettings;

    public UserAccountController(IMediator mediator, IAuthService authenticationService, IOptions<JwtSettings> jwtSettings)
    {
        _mediator = mediator;
        _authenticationService = authenticationService;
        _jwtSettings = jwtSettings.Value;
    }
   
    /// <summary>
    /// Register a new account
    /// </summary>
    /// <param name="registrationRequest"></param>
    /// <returns></returns>
    [HttpPost("Register")]
    [AllowAnonymous]
    public async Task<IActionResult> Register([FromBody] RegistrationRequest registrationRequest)
    {
        return Ok(await _authenticationService.RegisterAsync(registrationRequest));
    }

    /// <summary>
    /// Login
    /// </summary>
    /// <param name="registrationRequest"></param>
    /// <returns></returns>
    [HttpPost("Login")]
    [AllowAnonymous]
    public async Task<LoginResponse> Login([FromBody] LoginRequest loginRequest)
    {
        var authReponse = await _authenticationService.LoginAsync(loginRequest);
        return authReponse;
    }
    /// <summary>
    /// Refresh login with jwt token. Jwt token much be active and refresh token must be active
    /// </summary>
    /// <returns></returns>
    [HttpPost("RefreshAuthWithJwtRefreshToken")]
    [AllowAnonymous]
    public async Task<IActionResult> RefreshAuthWithJwtRefreshToken([FromBody] RefreshLoginRequest refreshLoginRequest)
    {
        return Ok(await _authenticationService.RefreshAuthWithJwtRefreshToken(refreshLoginRequest));
    }
    /// <summary>
    /// Revoke refresh token
    /// </summary>
    /// <returns></returns>
    [HttpPost("RevokeJwtRefreshToken")]
    public async Task<IActionResult> RevokeJwtRefreshToken([FromBody] RevokeJwtRefreshTokenRequest revokeJwtRefreshTokenRequest)
    {
        await _authenticationService.RevokeJwtRefreshToken(revokeJwtRefreshTokenRequest.UserName, "Revoked");
        return Ok();
    }
    /// <summary>
    /// Logout
    /// </summary>
    /// <returns></returns>
    [HttpPost("Logout")]
    [AllowAnonymous]
    public async Task<IActionResult> Logout()
    {
        return Ok(await _authenticationService.LogoutAsync());
    }
    

}
