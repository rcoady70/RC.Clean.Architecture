using RC.CA.Application.Models;

namespace RC.CA.Application.Dto.Cdn;
public class CsvFilesListResponseDto : BaseResponseCAResult
{
    public string? FilterByName { get; set; } = "";
    public string? FilterById { get; set; } = "";
    public IReadOnlyList<CsvFileListDto> CsvFiles { get; set; } = new List<CsvFileListDto>();
    public PaginationMetaData PaginationMetaData { get; set; } = new PaginationMetaData();
}
