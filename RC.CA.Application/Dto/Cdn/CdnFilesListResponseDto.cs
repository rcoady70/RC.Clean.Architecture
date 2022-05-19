using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RC.CA.Application.Models;

namespace RC.CA.Application.Dto.Cdn;
public class CdnFilesListResponseDto : BaseResponseDto
{
    public string? FilterByName { get; set; } = "";
    public string? FilterById { get; set; } = "";
    public IReadOnlyList<CdnListDto> Files { get; set; } = new List<CdnListDto>();
    public PaginationMetaData PaginationMetaData { get; set; } = new PaginationMetaData();
}
public class CdnListDto
{
    public Guid Id { get; set; } = default;
    public string FileName { get; set; } = "";
    public string OrginalFileName { get; set; } = "";
    public long FileSize { get; set; } = 0;
    public string ContentType { get; set; } = "";
    public string CdnLocation { get; set; } = "";
    public string? CreatedBy { get; set; } = default!;
    public DateTime? CreatedOn { get; set; } = default;
    public string? UpdatedBy { get; set; } = default!;
    public DateTime? UpdatedOn { get; set; } = default;
}
