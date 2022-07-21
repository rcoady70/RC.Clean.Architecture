using MediatR;
using RC.CA.Application.Dto.Cdn;

namespace RC.CA.Application.Features.Cdn.Queries
{
    public class UpsertCsvMapRequest : IRequest<CAResult<UpsertCsvMapResponseDto>>, IServiceRequest
    {
        public Guid Id { get; set; } = default;
        public List<CsvColumnMapDto> ColumnMap { get; set; } = new List<CsvColumnMapDto>();
    }
}
