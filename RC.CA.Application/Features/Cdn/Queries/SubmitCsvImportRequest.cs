using MediatR;

namespace RC.CA.Application.Features.Cdn.Queries
{
    public class SubmitCsvImportRequest : IRequest<CAResultEmpty>, IServiceRequest
    {
        public Guid Id { get; set; }
    }
}
