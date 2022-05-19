using RC.CA.Application.Models;
using System.Text.Json.Serialization;


namespace RC.CA.Application.Dto.Authentication;

public class LoginResponse : BaseResponseDto
{
    [JsonPropertyName("expires_at")]
    public DateTime? ExpiresAt { get; set; }
    [JsonPropertyName("access_token")]
    public string? AccessToken { get; set; }
    public string RefreshToken { get; set; }
}
