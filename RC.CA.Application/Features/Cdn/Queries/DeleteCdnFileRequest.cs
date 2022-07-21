using MediatR;

namespace RC.CA.Application.Features.Cdn.Queries
{
    public class DeleteCdnFileRequest : IRequest<CAResultEmpty>, IServiceRequest
    {
        public Guid Id { get; set; }
    }
}
