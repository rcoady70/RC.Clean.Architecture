using RC.CA.Application.Models;

namespace RC.CA.Application.Dto.Cdn
{
    public class UpsertCsvMapResponseDto : BaseResponseDto
    {
        public Guid Id { get; set; } = default;
        public List<CsvColumnMapDto> ColumnMap { get; set; } = new List<CsvColumnMapDto>();
    }
}
