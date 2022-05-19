using MediatR;
using RC.CA.Application.Dto;
using RC.CA.Application.Dto.Club;
using RC.CA.SharedKernel.Constants;

namespace RC.CA.Application.Features.Club.Queries;
/// <summary>
/// Get member list request, out IReadOnlyList<MemberListResponseDto>
/// </summary>
public class GetMemberRequest : IRequest<GetMemberResponseDto>
{
    public Guid? Id { get; set; }
}
