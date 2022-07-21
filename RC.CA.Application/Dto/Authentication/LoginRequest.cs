using System.ComponentModel.DataAnnotations;

namespace RC.CA.Application.Dto.Authentication;

public class LoginRequest : IServiceRequest
{
    public string UserEmail { get; set; } = default!;
    [DataType(DataType.Password)]
    public string Password { get; set; } = default!;
    public bool RememberMe { get; set; }
    public string? ReturnUrlX { get; set; }

}

