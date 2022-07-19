using MediatR;
using RC.CA.Application.Models;

namespace RC.CA.Application.Features.Club.Queries;
public class DeleteMemberRequest : IRequest<CAResult<BaseResponseDto>>
{
    public Guid Id { get; set; }
}
