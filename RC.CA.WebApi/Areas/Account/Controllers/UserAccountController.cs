using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using RC.CA.Application.Contracts.Identity;
using RC.CA.Application.Dto.Authentication;
using RC.CA.Application.Settings;

namespace RC.CA.WebApi.Areas.Account.Controllers;
[Route("api/[controller]")]
[ApiController]
public class UserAccountController : BaseController
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
    public async Task<ActionResult<CAResult<RegistrationResponse>>> Register([FromBody] RegistrationRequest registrationRequest)
    {
        return HandleResult(await _authenticationService.RegisterAsync(registrationRequest));
    }

    /// <summary>
    /// Login
    /// </summary>
    /// <param name="registrationRequest"></param>
    /// <returns></returns>
    [HttpPost("Login")]
    [AllowAnonymous]
    public async Task<ActionResult<CAResult<LoginResponse>>> Login([FromBody] LoginRequest loginRequest)
    {
        var authReponse = await _authenticationService.LoginAsync(loginRequest);
        return HandleResult(authReponse);
    }
    /// <summary>
    /// Refresh login with jwt token. Jwt token much be active and refresh token must be active
    /// </summary>
    /// <returns></returns>
    [HttpPost("RefreshAuthWithJwtRefreshToken")]
    [AllowAnonymous]
    public async Task<ActionResult<CAResult<LoginResponse>>> RefreshAuthWithJwtRefreshToken([FromBody] RefreshLoginRequest refreshLoginRequest)
    {
        return HandleResult(await _authenticationService.RefreshAuthWithJwtRefreshToken(refreshLoginRequest));
    }
    /// <summary>
    /// Revoke refresh token
    /// </summary>
    /// <returns></returns>
    [HttpPost("RevokeJwtRefreshToken")]
    public async Task<ActionResult<CAResultEmpty>> RevokeJwtRefreshToken([FromBody] RevokeJwtRefreshTokenRequest revokeJwtRefreshTokenRequest)
    {
        return HandleResult(await _authenticationService.RevokeJwtRefreshToken(revokeJwtRefreshTokenRequest.UserName, "Revoked"));
    }
    /// <summary>
    /// Logout
    /// </summary>
    /// <returns></returns>
    [HttpPost("Logout")]
    [AllowAnonymous]
    public async Task<ActionResult<CAResultEmpty>> Logout()
    {
        return HandleResult(await _authenticationService.LogoutAsync());
    }
}
