using System.Text.Json.Serialization;
using RC.CA.Domain.Entities.Common;

namespace RC.CA.Domain.Entities.Account;
//https://jasonwatmore.com/post/2021/06/15/net-5-api-jwt-authentication-with-refresh-tokens#jwt-utils-cs

public class JwtRefreshToken: BaseEntity<Guid>
{
    public string UserId { get; set; }
    public string Token { get; set; } = string.Empty;
    public DateTime ExpiresOn { get; set; }
    public string CreatedByIp { get; set; } = string.Empty;
    public DateTime? RevokedOn { get; set; }
    public string? RevokedByIp { get; set; }
    public string? ReasonRevoked { get; set; }
    public bool IsExpired => DateTime.UtcNow >= ExpiresOn;
    public bool IsRevoked => RevokedOn != null;
    public bool IsActive => !IsRevoked && !IsExpired;
}

