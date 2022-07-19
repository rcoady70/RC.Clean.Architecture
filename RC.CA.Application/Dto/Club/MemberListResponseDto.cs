using RC.CA.Application.Models;

namespace RC.CA.Application.Dto.Club;
public class MemberListResponseDto : BaseResponseCAResult
{
    public string? FilterByName { get; set; } = "";
    public string? FilterById { get; set; } = "";
    public IReadOnlyList<MemberListDto> Members { get; set; } = new List<MemberListDto>();
    public PaginationMetaData PaginationMetaData { get; set; } = new PaginationMetaData();
}
public class MemberListDto
{
    public Guid Id { get; set; }

    public string Name { get; set; } = "";

    public string Gender { get; set; } = "";

    public string? PhotoUrl { get; set; } = "";
    public DateTime? CreatedOn { get; set; }
    public string? CreatedBy { get; set; } = "";
    public DateTime? UpdatedOn { get; set; }
    public string? UpdatedBy { get; set; } = "";
}
