using MediatR;
using RC.CA.Application.Dto;
using RC.CA.Application.Dto.Cdn;
using RC.CA.Application.Features.Cdn.Queries;

namespace RC.CA.Application.Features.Cdn.Handlers
{
    public class GetCdnTestMethodRequestHandler : IRequestHandler<GetCdnTestMethodRequest, ApiResultDto<GetCdnTestMethodResponseDto>>
    {

        public async Task<ApiResultDto<GetCdnTestMethodResponseDto>> Handle(GetCdnTestMethodRequest request, CancellationToken cancellationToken)
        {
            var response = new GetCdnTestMethodResponseDto();
            return ApiResultDto<GetCdnTestMethodResponseDto>.Success(response);
        }
    }
}
