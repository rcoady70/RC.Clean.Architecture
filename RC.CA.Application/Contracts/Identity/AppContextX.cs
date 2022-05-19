using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RC.CA.Application.Contracts.Identity;
/// <summary>
/// Application context hydrated using AppContextFilter on each request
/// </summary>
public class AppContextX : IAppContextX
{
    public string UserName { get; set; } = "";
    public string? UserId { get; set; } = "";
    public bool IsAuthenticated { get; set; } = false;
    public string JwtToken { get; set; } = "";
    public string IpAddress { get; set; } = "";
    public string? CorrelationId { get; set; } = "";
    public string? CspNouce { get; set; } = "";
    public string? CdnEndpoint { get; set; }
    public string? ApiEndpoint { get; set; }
}

