using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using RC.CA.Application.Contracts.Identity;
using RC.CA.Application.Contracts.Persistence;
using RC.CA.Application.Dto.Authentication;
using RC.CA.Application.Settings;
using RC.CA.Domain.Entities.Account;
using RC.CA.Infrastructure.Persistence.AuthorizationJwt;
using RC.CA.Infrastructure.Persistence.Identity;

namespace RC.CA.Infrastructure.Persistence.Services;


/// <summary>
/// Authentication service
/// </summary>
public class AuthService : IAuthService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    public readonly IAppContextX _appContextX;
    public readonly RoleManager<IdentityRole> _roleManager;
    private readonly IJwtUtilities _jwtUtilities;
    private readonly IAsyncRepository<JwtRefreshToken> _jwtRefreshTokenRepository;
    private readonly JwtSettings _jwtSettings;


    public AuthService(UserManager<ApplicationUser> userManager,
                        SignInManager<ApplicationUser> signInManager,
                        IAppContextX appContextX,
                        RoleManager<IdentityRole> roleManager,
                        IOptions<JwtSettings> jwtSettings,
                        IJwtUtilities jwtUtilities,
                        IAsyncRepository<JwtRefreshToken> jwtRefreshTokenRepository)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _appContextX = appContextX;
        _roleManager = roleManager;
        _jwtUtilities = jwtUtilities;
        _jwtRefreshTokenRepository = jwtRefreshTokenRepository;
        _jwtSettings = jwtSettings.Value;
    }
    /// <summary>
    /// Logout
    /// </summary>
    /// <returns></returns>
    public async Task<CAResultEmpty> LogoutAsync()
    {
        await _signInManager.SignOutAsync();
        return CAResultEmpty.Success();
    }
    /// <summary>
    /// Refresh login with JWT token
    /// </summary>
    /// <returns></returns>
    public async Task<CAResult<LoginResponse>> RefreshAuthWithJwtRefreshToken(RefreshLoginRequest refreshLoginRequest)
    {
        LoginResponse response = new LoginResponse();
        var tokenExpiresAt = DateTime.UtcNow.AddMinutes(_jwtSettings.DurationInMinutes);

        response.AccessToken = await _jwtUtilities.RefreshJwtToken(refreshLoginRequest, tokenExpiresAt);
        if (response.AccessToken == null)
            return CAResult<LoginResponse>.Unauthorized("LoginFailed", $"Login refresh failed please try to login again", ValidationSeverity.Error);
        else
        {
            //Add new refresh token to database
            response.RefreshToken = _jwtUtilities.GenerateRefreshToken();
            response.ExpiresAt = tokenExpiresAt;
            await _jwtUtilities.SaveJwtRefreshToken(response.RefreshToken, refreshLoginRequest.UserName, "Refresh token");
            return CAResult<LoginResponse>.Success(response);
        }
    }
    public async Task<CAResultEmpty> RevokeJwtRefreshToken(string userId, string reason)
    {
        await _jwtUtilities.RevokeJwtRefreshToken(userId, reason);
        return CAResultEmpty.Success();
    }
    /// <summary>
    /// Login
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public async Task<CAResult<LoginResponse>> LoginAsync(LoginRequest request)
    {
        //Get user
        var user = await _userManager.FindByEmailAsync(request.UserEmail);
        if (user == null)
            return CAResult<LoginResponse>.Unauthorized("LoginFailed", $"User with {request.UserEmail} not found.", ValidationSeverity.Error);

        //Sign in user based on user and password
        var result = await _signInManager.PasswordSignInAsync(request.UserEmail, request.Password, false, lockoutOnFailure: false);
        if (!result.Succeeded)
            return CAResult<LoginResponse>.Unauthorized("LoginFailed", $"Credentials for '{request.UserEmail} aren't valid'.", ValidationSeverity.Error);

        //Generate jwt token
        LoginResponse response = new LoginResponse();
        var tokenExpiresAt = DateTime.UtcNow.AddMinutes(_jwtSettings.DurationInMinutes);
        string jwtSecurityToken = await _jwtUtilities.GenerateJwtTokenAsync(user, tokenExpiresAt);
        response.AccessToken = jwtSecurityToken;
        response.RefreshToken = _jwtUtilities.GenerateRefreshToken();
        await _jwtUtilities.SaveJwtRefreshToken(response.RefreshToken, user.UserName, "Login");
        CAResult<LoginResponse>.Success(response);

        return response;
    }
    /// <summary>
    /// Register new account
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public async Task<CAResult<RegistrationResponse>> RegisterAsync(RegistrationRequest request)
    {
        var user = new ApplicationUser
        {
            Email = $"{request.Email}",
            FirstName = request.FirstName,
            LastName = request.LastName,
            UserName = request.Email,
            EmailConfirmed = false,
        };

        //Ensure default role exists
        if (!_roleManager.RoleExistsAsync(RC.CA.SharedKernel.Constants.DB.RoleAdmin).GetAwaiter().GetResult())
        {
            _roleManager.CreateAsync(new IdentityRole(RC.CA.SharedKernel.Constants.DB.RoleAdmin)).GetAwaiter().GetResult();
            _roleManager.CreateAsync(new IdentityRole(RC.CA.SharedKernel.Constants.DB.RoleUser)).GetAwaiter().GetResult();
        }

        //Add user
        RegistrationResponse response = new RegistrationResponse();

        var result = await _userManager.CreateAsync(user, request.Password);
        if (result.Succeeded)
        {
            await _userManager.AddToRoleAsync(user, RC.CA.SharedKernel.Constants.DB.RoleAdmin);
            var userId = await _userManager.GetUserIdAsync(user);
            response.UserId = userId;
        }
        else
        {
            var failedResponse = CAResult<RegistrationResponse>.Invalid();
            foreach (var error in result.Errors)
                failedResponse.AddValidationError(error.Code, error.Description, ValidationSeverity.Error);
            return failedResponse;
        }
        return CAResult<RegistrationResponse>.Success(response);
    }
}
