using RC.CA.Application.Models;

namespace RC.CA.Application.Dto.Authentication;

public class RegistrationResponse : BaseResponseDto
{
    public string? UserId { get; set; }

}
