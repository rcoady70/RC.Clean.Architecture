using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using RC.CA.Domain.Entities.Account;

namespace RC.CA.Infrastructure.Persistence.Identity;

public class ApplicationUser : IdentityUser
{
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    [JsonIgnore]
    public List<JwtRefreshToken> RefreshToken { get; set; }= default!;
}
