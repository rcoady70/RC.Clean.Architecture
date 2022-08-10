using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RC.CA.Application.Dto.Cdn;
using RC.CA.Application.Features.Cdn.Queries;
using RC.CA.Infrastructure.MessageBus;

namespace RC.CA.WebApi.Areas.CDN
{
    [Route("api/cdn/[controller]")]
    [ApiController]
    public class ImageController : BaseController
    {

        public IMediator _mediator { get; }
        public ImageController(IMediator mediator)
        {
            _mediator = mediator;
        }
        public class TestIntegrationMessage : IntegrationMessage
        {
            public int OrderId { get; }

            public string Description { get; }

            public TestIntegrationMessage(int orderId, string description)
            {
                OrderId = orderId;
                Description = description;
            }
        }

        [HttpGet("List")]
        public async Task<ActionResult<CAResult<CdnFilesListResponseDto>>> List(GetCdnFilesListRequest getCdnFilesListRequest, CancellationToken cancellationToken)
        {
            return HandleResult(await _mediator.Send(getCdnFilesListRequest));
        }
        /// <summary>
        /// Process uploaded file.
        /// </summary>
        /// <param name="filedata">This references a variable name in the jquery.filedrop.js</param>
        /// <returns></returns>
        [HttpPost("Upload")]
        [AllowAnonymous]
        public async Task<ActionResult<CAResult<CreateCdnFileResponseDto>>> Upload([FromForm] IFormFile? fileData, CancellationToken cancellationToken)
        {
            CreateCdnFileResponseDto response = new CreateCdnFileResponseDto();
            if (fileData != null)
            {
                CreateCdnFileRequest createUploadedFilesRequest = new CreateCdnFileRequest();
                createUploadedFilesRequest.UploadedFile = fileData;
                return HandleResult(await _mediator.Send(createUploadedFilesRequest));
            }
            else
                return HandleResult(CAResult<CreateCdnFileResponseDto>.Invalid("UploadFailed", "Uploaded file not found", ValidationSeverity.Error));
        }
        /// <summary>
        /// Delete image
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpDelete("Delete")]
        public async Task<ActionResult<CAResultEmpty>> Delete(DeleteCdnFileRequest deleteCdnFileRequest, CancellationToken cancellationToken)
        {
            Guard.Against.Null(deleteCdnFileRequest, nameof(deleteCdnFileRequest));

            return HandleResult(await _mediator.Send(deleteCdnFileRequest));
        }
    }
}
