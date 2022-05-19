using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace RC.CA.Application.Dto.Authentication;

public class CorsSettings
{
    public string[] AllowedOrgins { get; set; }
    
}
