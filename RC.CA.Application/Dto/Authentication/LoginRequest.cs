using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RC.CA.Application.Dto.Authentication;

public  class LoginRequest
{
    public string UserEmail { get; set; } = default!;
    [DataType(DataType.Password)]
    public string Password { get; set; } = default!;
    public bool RememberMe { get; set; }
    public string? ReturnUrlX { get; set; }

}

