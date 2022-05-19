using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RC.CA.Application.Models;

namespace RC.CA.Application.Dto.Authentication;

public class RegistrationResponse:BaseResponseDto
{
    public string? UserId { get; set; }

}
