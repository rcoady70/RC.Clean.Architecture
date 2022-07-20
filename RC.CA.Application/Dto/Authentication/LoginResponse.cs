using System.Text.Json.Serialization;
using RC.CA.Application.Models;


namespace RC.CA.Application.Dto.Authentication;

public class LoginResponse : BaseResponseCAResult
{
    [JsonPropertyName("expires_at")]
    public DateTime? ExpiresAt { get; set; }
    [JsonPropertyName("access_token")]
    public string? AccessToken { get; set; }
    public string RefreshToken { get; set; }
}
