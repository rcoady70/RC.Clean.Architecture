using MediatR;
using RC.CA.Application.Contracts.Persistence;
using RC.CA.Application.Dto.Club;

namespace RC.CA.Application.Features.Club.Queries;
/// <summary>
/// Get member list request, out IReadOnlyList<MemberListResponseDto>
/// </summary>
public class GetMemberListRequest : IRequest<CAResult<MemberListResponseDto>>, IServiceRequest, IIsCachableMediatrResult
{

    public string? FilterByName { get; set; } = "";
    public string? FilterById { get; set; } = "";
    public string? OrderBy { get; set; } = "";
    public int PageSeq { get; set; } = 1;
    public int PageSize { get; set; } = DB.ListItemsPerPage;
    public bool CacheSkip { get; set; } = true;
    public string CacheKey => $"MemberListRequest-{FilterByName}-{FilterById}-{OrderBy}-{PageSeq}-{PageSize}";
    public int CacheSlidingExpirationInMin { get; set; } = 5;
}
