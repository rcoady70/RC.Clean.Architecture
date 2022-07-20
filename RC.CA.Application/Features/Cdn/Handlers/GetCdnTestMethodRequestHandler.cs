using MediatR;
using RC.CA.Application.Dto.Cdn;
using RC.CA.Application.Features.Cdn.Queries;

namespace RC.CA.Application.Features.Cdn.Handlers
{
    public class GetCdnTestMethodRequestHandler : IRequestHandler<GetCdnTestMethodRequest, CAResult<GetCdnTestMethodResponseDto>>
    {

        public async Task<CAResult<GetCdnTestMethodResponseDto>> Handle(GetCdnTestMethodRequest request, CancellationToken cancellationToken)
        {
            var response = new GetCdnTestMethodResponseDto();
            return CAResult<GetCdnTestMethodResponseDto>.Success(response);
        }
    }
}
