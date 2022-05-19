using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RC.CA.Application.Contracts.Persistence;
using RC.CA.Domain.Entities.Cdn;

namespace RC.CA.Infrastructure.Persistence.Repository
{
    public class CdnFileRepository : AsyncRepository<CdnFiles>, ICdnFileRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public CdnFileRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
    }
}
