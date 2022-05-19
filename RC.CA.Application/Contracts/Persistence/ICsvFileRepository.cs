using System;
using RC.CA.Domain.Entities.Cdn;
using RC.CA.Domain.Entities.CSV;

namespace RC.CA.Application.Contracts.Persistence;
public interface ICsvFileRepository : IAsyncRepository<CsvFile>
{
}

