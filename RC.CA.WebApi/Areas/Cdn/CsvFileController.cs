using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace RC.CA.WebApi.Areas.Cdn;

    [Route("api/[controller]")]
    [ApiController]
public class CsvFileController : ControllerBase
{
    /// <summary>
    /// Process csv uploaded file.
    /// </summary>
    /// <param name="filedata">This references a variable name in the jquery.filedrop.js</param>
    /// <returns></returns>
    //[HttpPost("Upload")]
    //[AllowAnonymous]
    //public async Task<CreateUploadedFilesResponseDto> Upload([FromForm] IFormFile? fileData)
    //{
    //    var cookie = Request.Cookies.Count;
    //    CreateUploadedFilesResponseDto response = new CreateUploadedFilesResponseDto();
    //    if (fileData != null)
    //    {
    //        CreateCdnFileRequestValidator validationRules = new CreateCdnFileRequestValidator();
    //        var valResult = validationRules.Validate(fileData);
    //        response.CheckFluentValidationResults(valResult);
    //        if (response.TotalErrors == 0)
    //        {
    //            CreateCdnFileRequest createUploadedFilesRequest = new CreateCdnFileRequest();
    //            createUploadedFilesRequest.UploadedFiles.Add(fileData);
    //            response = await _mediator.Send(createUploadedFilesRequest);
    //        }
    //    }
    //    else
    //    {
    //        response = new CreateUploadedFilesResponseDto();
    //        response.AddResponseError(Application.Models.BaseResponseDto.ErrorType.Error, "Uploaded file not found");
    //    }
    //    return response;
    //}
}
