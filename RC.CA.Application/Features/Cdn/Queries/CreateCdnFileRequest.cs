using MediatR;
using Microsoft.AspNetCore.Http;
using RC.CA.Application.Dto.Cdn;

namespace RC.CA.Application.Features.Cdn.Queries
{
    public class CreateCdnFileRequest : IRequest<CAResult<CreateCdnFileResponseDto>>
    {
        public IFormFile? UploadedFile { get; set; } = default;
    }
}
