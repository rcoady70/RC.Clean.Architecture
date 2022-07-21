using MediatR;

namespace RC.CA.Application.Features.Club.Queries;
public class DeleteMemberRequest : IRequest<CAResultEmpty>, IServiceRequest
{
    public Guid Id { get; set; }
}
