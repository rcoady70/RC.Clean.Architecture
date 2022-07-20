using MediatR;

namespace RC.CA.Application.Features.Club.Queries;
public class DeleteMemberRequest : IRequest<CAResultEmpty>
{
    public Guid Id { get; set; }
}
