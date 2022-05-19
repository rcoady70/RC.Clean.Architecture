using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RC.CA.SharedKernel.Constants;
/// <summary>
/// Constants related to sessions and claims RC.CA.SharedKernel.Constants.SessionConstants
/// </summary>
public static class SessionConstants
{
    public const string Session_Cookie = "RCCAApplication";
    public const int Session_Cookie_ExpiryInMin = 60;
    public const string Session_Cookie_AuthScheme = "Identity.Application";
    public const string ClaimUserId = "uid";

    //https://brokul.dev/authentication-cookie-lifetime-and-sliding-expiration
    public const int Session_Cookie_MaxLifeTimeInHours = 8; //Max lifetime of a cookie
    public const int Session_Jwt_RefreshTokenInHours = 24; //Max jwt refresh token
}
