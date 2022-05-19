using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
          
namespace RC.CA.Application.Contracts.Identity;
/// <summary>
/// Application context hydrated using AppContextFilter on each request
/// </summary>
public interface IAppContextX
{
    string UserName { get; set; }
    string? UserId { get; set; }
    bool IsAuthenticated { get; set; }
    string JwtToken { get; set; }
    string IpAddress { get; set; }
    string? CorrelationId { get; set; }
    string? CspNouce { get; set; }
    string? CdnEndpoint { get; set; }
    string? ApiEndpoint { get; set; }
}
