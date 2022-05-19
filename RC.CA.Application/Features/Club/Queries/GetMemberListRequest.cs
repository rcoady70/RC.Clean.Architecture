using MediatR;
using RC.CA.Application.Dto;
using RC.CA.Application.Dto.Club;
using RC.CA.SharedKernel.Constants;

namespace RC.CA.Application.Features.Club.Queries;
/// <summary>
/// Get member list request, out IReadOnlyList<MemberListResponseDto>
/// </summary>
public class GetMemberListRequest : IRequest<MemberListResponseDto>
{
    public string? FilterByName { get; set; } = "";
    public string? FilterById { get; set; } = "";
    public string? OrderBy { get; set; } = "";
    public int PageSeq { get; set; } = 1;
    public int PageSize { get; set; } = DB.ListItemsPerPage;
    public async Task<int> Test()
    {
        return 1;
    }
}
