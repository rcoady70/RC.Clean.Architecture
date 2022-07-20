using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RC.CA.Application.Dto.Cdn;
using RC.CA.Application.Features.Cdn.Queries;
using RC.CA.Application.Features.Club.Queries;
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
        public async Task<CAResult<CdnFilesListResponseDto>> List(GetCdnFilesListRequest getCdnFilesListRequest)
        {
            CdnFilesListResponseDto response = new CdnFilesListResponseDto();
            GetCdnFilesRequestValidator validationRules = new GetCdnFilesRequestValidator();
            var valResult = validationRules.Validate(getCdnFilesListRequest);
            //Check result of model validation
            if (valResult.IsValid)
                return await _mediator.Send(getCdnFilesListRequest);
            else
                return CAResult<CdnFilesListResponseDto>.Invalid(valResult.AsModelStateErrors());
        }
        /// <summary>
        /// Process uploaded file.
        /// </summary>
        /// <param name="filedata">This references a variable name in the jquery.filedrop.js</param>
        /// <returns></returns>
        [HttpPost("Upload")]
        [AllowAnonymous]
        public async Task<CAResult<CreateCdnFileResponseDto>> Upload([FromForm] IFormFile? fileData)
        {
            var cookie = Request.Cookies.Count;
            CreateCdnFileResponseDto response = new CreateCdnFileResponseDto();
            if (fileData != null)
            {
                CreateCdnFileRequestValidator validationRules = new CreateCdnFileRequestValidator();
                var valResult = validationRules.Validate(fileData);
                if (valResult.IsValid)
                {
                    CreateCdnFileRequest createUploadedFilesRequest = new CreateCdnFileRequest();
                    createUploadedFilesRequest.UploadedFile = fileData;
                    return await _mediator.Send(createUploadedFilesRequest);
                }
                else
                    return CAResult<CreateCdnFileResponseDto>.Invalid(valResult.AsModelStateErrors());
            }
            else
                return CAResult<CreateCdnFileResponseDto>.Invalid("UploadFailed", "Uploaded file not found", ValidationSeverity.Error);
        }
        /// <summary>
        /// Delete image
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpDelete("Delete")]
        public async Task<CAResultEmpty> Delete(DeleteCdnFileRequest deleteCdnFileRequest)
        {
            Guard.Against.Null(deleteCdnFileRequest, nameof(deleteCdnFileRequest));

            return await _mediator.Send(deleteCdnFileRequest);

        }
    }
}
