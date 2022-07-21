using MediatR;
using RC.CA.Application.Dto.Club;

namespace RC.CA.Application.Features.Club.Queries;
/// <summary>
/// Get member list request, out IReadOnlyList<MemberListResponseDto>
/// </summary>
public class GetMemberRequest : IRequest<CAResult<GetMemberResponseDto>>, IServiceRequest
{
    public Guid? Id { get; set; }
}
