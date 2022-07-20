using MediatR;
using Microsoft.AspNetCore.Http;
using RC.CA.Application.Dto.Cdn;

namespace RC.CA.Application.Features.Cdn.Queries
{
    public class CreateCSvFileRequest : IRequest<CAResult<CreateCsvFileResponseDto>>
    {
        public IFormFile? UploadedFile { get; set; }
    }
}
