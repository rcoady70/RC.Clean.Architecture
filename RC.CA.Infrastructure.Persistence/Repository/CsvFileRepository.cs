using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RC.CA.Application.Contracts.Persistence;
using RC.CA.Domain.Entities.CSV;

namespace RC.CA.Infrastructure.Persistence.Repository
{
    public class CsvFileRepository : AsyncRepository<CsvFile>, ICsvFileRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public CsvFileRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
    }
}
