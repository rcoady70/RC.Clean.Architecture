using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RC.CA.Application.Dto.Authentication;
using RC.CA.Application.Models;

namespace RC.CA.Application.Contracts.Identity;
/// <summary>
/// Authentication service
/// </summary>
public interface IAuthService
{
    Task<LoginResponse> LoginAsync(LoginRequest request);
    Task<BaseResponseDto> LogoutAsync();
    Task<LoginResponse> RefreshAuthWithJwtRefreshToken(RefreshLoginRequest refreshLoginRequest);
    Task RevokeJwtRefreshToken(string UserId,string Reason);
    Task<RegistrationResponse> RegisterAsync(RegistrationRequest request);
}
