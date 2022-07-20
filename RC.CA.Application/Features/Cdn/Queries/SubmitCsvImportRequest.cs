using MediatR;

namespace RC.CA.Application.Features.Cdn.Queries
{
    public class SubmitCsvImportRequest : IRequest<CAResultEmpty>
    {
        public Guid Id { get; set; }
    }
}
