using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RC.CA.Application.Dto.Cdn;
using RC.CA.Application.Features.Cdn.Queries;
using RC.CA.Application.Features.Club.Queries;
using RC.CA.Application.Models;
using RC.CA.Infrastructure.MessageBus;
using RC.CA.Infrastructure.MessageBus.Interfaces;
using RC.CA.SharedKernel.Constants;
using RC.CA.SharedKernel.GuardClauses;

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

            public TestIntegrationMessage(int orderId,string description)
            {
                OrderId = orderId;
                Description = description;
            }
        }
       
        [HttpGet("List")]
        public async Task<CdnFilesListResponseDto> List(GetCdnFilesListRequest getCdnFilesListRequest)
        {
            CdnFilesListResponseDto response = new CdnFilesListResponseDto();
            GetCdnFilesRequestValidator validationRules = new GetCdnFilesRequestValidator();
            var valResult = validationRules.Validate(getCdnFilesListRequest);
            response.CheckFluentValidationResults(valResult);

            //Check result of model validation
            if (response.TotalErrors == 0)
                response = await _mediator.Send(getCdnFilesListRequest);
            else
                return InvalidRequest(response);

            return response;
        }

        

        /// <summary>
        /// Process uploaded file.
        /// </summary>
        /// <param name="filedata">This references a variable name in the jquery.filedrop.js</param>
        /// <returns></returns>
        [HttpPost("Upload")]
        [AllowAnonymous]
        public async Task<CreateCdnFileResponseDto> Upload([FromForm]IFormFile? fileData)
        {
            var cookie = Request.Cookies.Count;
            CreateCdnFileResponseDto response = new CreateCdnFileResponseDto();
            if (fileData != null)
            {
                CreateCdnFileRequestValidator validationRules = new CreateCdnFileRequestValidator();
                var valResult = validationRules.Validate(fileData);
                response.CheckFluentValidationResults(valResult);
                if (response.TotalErrors == 0)
                {
                    CreateCdnFileRequest createUploadedFilesRequest = new CreateCdnFileRequest();
                    createUploadedFilesRequest.UploadedFile = fileData;
                    response = await _mediator.Send(createUploadedFilesRequest);
                }
            }
            else
            {
                response = new CreateCdnFileResponseDto();
                response.AddResponseError( Application.Models.BaseResponseDto.ErrorType.Error,"Uploaded file not found");
            }
            return response;
        }
        /// <summary>
        /// Delete image
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpDelete("Delete")]
        public async Task<BaseResponseDto> Delete(DeleteCdnFileRequest deleteCdnFileRequest)
        {
            Guard.Against.Null(deleteCdnFileRequest, nameof(deleteCdnFileRequest));

            var baseResponse = await _mediator.Send(deleteCdnFileRequest);
           
            return baseResponse;
        }
    }
}
