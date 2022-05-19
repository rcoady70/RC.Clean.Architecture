using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RC.CA.Application.Contracts.Persistence;
using RC.CA.Domain.Entities.Account;

namespace RC.CA.Application.Contracts.Persistence;

public interface IJwtRefreshTokenRepository : IAsyncRepository<JwtRefreshToken>
{
    Domain.Entities.Account.JwtRefreshToken GetLatestRefreshTokenByUser(string userId);
}
