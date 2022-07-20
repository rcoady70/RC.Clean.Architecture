using RC.CA.Application.Dto.Authentication;

namespace RC.CA.Application.Contracts.Identity;
/// <summary>
/// Authentication service
/// </summary>
public interface IAuthService
{
    Task<CAResult<LoginResponse>> LoginAsync(LoginRequest request);
    Task<CAResultEmpty> LogoutAsync();
    Task<CAResult<LoginResponse>> RefreshAuthWithJwtRefreshToken(RefreshLoginRequest refreshLoginRequest);
    Task<CAResult<RegistrationResponse>> RegisterAsync(RegistrationRequest request);
    Task<CAResultEmpty> RevokeJwtRefreshToken(string userId, string reason);
}
