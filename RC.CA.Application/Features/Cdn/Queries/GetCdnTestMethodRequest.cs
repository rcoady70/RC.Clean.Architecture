using MediatR;
using RC.CA.Application.Dto.Cdn;

namespace RC.CA.Application.Features.Cdn.Queries
{
    public class GetCdnTestMethodRequest : IRequest<CAResult<GetCdnTestMethodResponseDto>>
    {
        public Guid guid { get; set; }
    }
}
