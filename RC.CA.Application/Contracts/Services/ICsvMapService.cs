using RC.CA.Domain.Entities.CSV;

namespace RC.CA.Application.Contracts.Services;
    public interface ICsvMapService
    {
        Task<CsvMap> BuildMapFromCshHead(Guid id);
    }
