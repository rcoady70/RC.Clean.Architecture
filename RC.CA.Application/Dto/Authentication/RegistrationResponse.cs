using RC.CA.Application.Models;

namespace RC.CA.Application.Dto.Authentication;

public class RegistrationResponse : BaseResponseCAResult
{
    public string? UserId { get; set; }

}
