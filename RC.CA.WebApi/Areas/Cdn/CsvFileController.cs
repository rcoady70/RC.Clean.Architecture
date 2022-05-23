using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RC.CA.Application.Dto.Cdn;
using RC.CA.Application.Features.Cdn.Queries;
using RC.CA.Application.Features.Club.Queries;

namespace RC.CA.WebApi.Areas.Cdn;

    [Route("api/[controller]")]
    [ApiController]
public class CsvFileController : ControllerBase
{
    private readonly IMediator _mediator;

    public CsvFileController(IMediator mediator)
    {
        _mediator = mediator;
    }
    /// <summary>
    /// Process csv uploaded file.
    /// </summary>
    /// <param name="filedata">This references a variable name in the jquery.filedrop.js</param>
    /// <returns></returns>
    [HttpPost("Upload")]
    [AllowAnonymous]
    public async Task<CreateCsvFileResponseDto> Upload([FromForm] IFormFile? fileData)
    {
        var cookie = Request.Cookies.Count;
        var response = new CreateCsvFileResponseDto();
        if (fileData != null)
        {
            CreateCdnFileRequestValidator validationRules = new CreateCdnFileRequestValidator();
            var valResult = validationRules.Validate(fileData);
            response.CheckFluentValidationResults(valResult);
            if (response.TotalErrors == 0)
            {
                CreateCSvFileRequest createUploadedFilesRequest = new CreateCSvFileRequest();
                createUploadedFilesRequest.UploadedFile = fileData;
                response = await _mediator.Send(createUploadedFilesRequest);
            }
        }
        else
        {
            response.AddResponseError(Application.Models.BaseResponseDto.ErrorType.Error, "Uploaded file not found");
        }
        return response;
    }
}
