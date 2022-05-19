using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RC.CA.Application.Dto.Authentication;

public class JwtSettings
{
    public string Key { get; set; } = default!;
    public string Issuer { get; set; } = default!;
    public string Audience { get; set; } = default!;
    public double DurationInMinutes { get; set; } = default!;
    public string RefreshCookieName { get; set; } = "RCJwtRefreshCookie";
    public int RefreshExpiryInDays { get; set; } = 7;
}
