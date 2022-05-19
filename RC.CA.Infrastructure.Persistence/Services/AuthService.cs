using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using RC.CA.Application.Contracts.Identity;
using RC.CA.Application.Contracts.Persistence;
using RC.CA.Application.Dto.Authentication;
using RC.CA.Domain.Entities.Account;
using RC.CA.Application.Models;
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
    public async Task<BaseResponseDto> LogoutAsync()
    {
        BaseResponseDto response = new BaseResponseDto();
        await _signInManager.SignOutAsync();
        return response;
    }
    /// <summary>
    /// Refresh login with JWT token
    /// </summary>
    /// <returns></returns>
    public async Task<LoginResponse> RefreshAuthWithJwtRefreshToken(RefreshLoginRequest refreshLoginRequest)
    {
        LoginResponse response = new LoginResponse();
        var tokenExpiresAt = DateTime.UtcNow.AddMinutes(_jwtSettings.DurationInMinutes);

        response.AccessToken = await _jwtUtilities.RefreshJwtToken(refreshLoginRequest, tokenExpiresAt);
        if (response.AccessToken == null)
            await response.AddResponseError(BaseResponseDto.ErrorType.Error, $"Login refresh failed please try to login again");
        else
        {
            //Add new refresh token to database
            response.RefreshToken = _jwtUtilities.GenerateRefreshToken();
            response.ExpiresAt = tokenExpiresAt;
            await _jwtUtilities.SaveJwtRefreshToken(response.RefreshToken, refreshLoginRequest.UserName,"Refresh token");
        }
        return response;
    }
    public async Task RevokeJwtRefreshToken(string userId, string reason)
    {
        await _jwtUtilities.RevokeJwtRefreshToken(userId, reason);
        return;
    }
    /// <summary>
    /// Login
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public async Task<LoginResponse> LoginAsync(LoginRequest request)
    {
        LoginResponse response = new LoginResponse();

        //Get user
        var user = await _userManager.FindByEmailAsync(request.UserEmail);
        if (user == null)
            await response.AddResponseError(BaseResponseDto.ErrorType.Error, $"User with {request.UserEmail} not found.");

        if (response.TotalErrors == 0)
        {
            //Sign in user based on user and password
            var result = await _signInManager.PasswordSignInAsync(request.UserEmail, request.Password, false, lockoutOnFailure: false);
            if (!result.Succeeded)
                await response.AddResponseError(BaseResponseDto.ErrorType.Error, $"Credentials for '{request.UserEmail} aren't valid'.");

            //Generate jwt token
            var tokenExpiresAt = DateTime.UtcNow.AddMinutes(_jwtSettings.DurationInMinutes);
            if (response.TotalErrors == 0)
            {
                string jwtSecurityToken = await _jwtUtilities.GenerateJwtTokenAsync(user, tokenExpiresAt);
                response.AccessToken = jwtSecurityToken;
                response.RefreshToken = _jwtUtilities.GenerateRefreshToken();
                await _jwtUtilities.SaveJwtRefreshToken(response.RefreshToken, user.UserName,"Login");
            }
        }
        return response;
    }
    /// <summary>
    /// Register new account
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public async Task<RegistrationResponse> RegisterAsync(RegistrationRequest request)
    {
        RegistrationResponse response = new RegistrationResponse();
        //
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
        var result = await _userManager.CreateAsync(user, request.Password);
        if (result.Succeeded)
        {
            await _userManager.AddToRoleAsync(user, RC.CA.SharedKernel.Constants.DB.RoleAdmin);
            var userId = await _userManager.GetUserIdAsync(user);
            response.UserId = userId;
        }
        else
        {
            foreach(var error in result.Errors)
                await response.AddResponseError(BaseResponseDto.ErrorType.Error, error.Description);
        }
        return response;
    }

   
}
