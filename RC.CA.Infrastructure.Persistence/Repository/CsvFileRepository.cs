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
        /// <summary>
        /// Check if import is being processed. Checks if status if running or being processed
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public bool IsBeingProcessed(Guid Id)
        {
            var file =  _dbContext.CsvFile.FirstOrDefault(c => c.Id == Id && (c.Status == FileStatus.OnQueue || c.Status == FileStatus.BeingProcessed));
            return file != null;
        }
    }
}
