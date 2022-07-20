using System.ComponentModel.DataAnnotations;

namespace RC.CA.Application.Dto.Authentication;

public class RegistrationRequest
{
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public string Email { get; set; } = default!;
    [DataType(DataType.Password)]
    public string Password { get; set; } = default!;
    [DataType(DataType.Password)]
    public string ConfirmPassword { get; set; } = default!;
}
