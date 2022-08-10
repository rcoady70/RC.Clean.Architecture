using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RC.CA.Application.Contracts.Persistence;
using RC.CA.Application.Dto.Cdn;
using RC.CA.Application.Features.Cdn.Queries;

namespace RC.CA.WebApi.Areas.Cdn;

[Route("api/[controller]")]
[ApiController]
public class CsvFileController : BaseController
{
    private readonly IMediator _mediator;
    private readonly ICsvFileRepository _csvFileRepository;

    public CsvFileController(IMediator mediator, ICsvFileRepository csvFileRepository)
    {
        _mediator = mediator;
        _csvFileRepository = csvFileRepository;
    }
    /// <summary>
    /// Process csv uploaded file.
    /// </summary>
    /// <param name="filedata">This references a variable name in the jquery.filedrop.js</param>
    /// <returns></returns>
    [HttpPost("Upload")]
    [AllowAnonymous]
    public async Task<ActionResult<CAResult<CreateCsvFileResponseDto>>> Upload([FromForm] IFormFile? fileData, CancellationToken cancellationToken)
    {
        if (fileData != null)
        {
            CreateCSvFileRequest createUploadedFilesRequest = new CreateCSvFileRequest();
            createUploadedFilesRequest.UploadedFile = fileData;
            var response = await _mediator.Send(createUploadedFilesRequest);
            return HandleResult(response);
        }
        else
            return HandleResult(CAResult<CreateCsvFileResponseDto>.Invalid("NoFile", "Uploaded file not found", ValidationSeverity.Error));
    }

    /// <summary>
    /// Get map based on id
    /// </summary>
    /// <param name="fileData"></param>
    /// <returns></returns>
    [HttpGet("GetMap")]
    public async Task<ActionResult<CAResult<UpsertCsvMapResponseDto>>> GetMap(GetCsvMapRequest getCsvMapRequest, CancellationToken cancellationToken)
    {
        return HandleResult(await _mediator.Send(getCsvMapRequest));
    }
    /// <summary>
    /// Get csv file list
    /// </summary>
    /// <param name="getCsvFilesListRequest"></param>
    /// <returns></returns>
    [HttpGet("List")]
    public async Task<ActionResult<CAResult<CsvFilesListResponseDto>>> List(GetCsvFileListRequest getCsvFilesListRequest, CancellationToken cancellationToken)
    {
        return HandleResult(await _mediator.Send(getCsvFilesListRequest));
    }
    /// <summary>
    /// Update map
    /// </summary>
    /// <param name="getCsvMapRequest"></param>
    /// <returns></returns>
    [HttpPatch("UpdateMap")]
    public async Task<ActionResult<CAResult<UpsertCsvMapResponseDto>>> UpdateMap(UpsertCsvMapRequest upsertCsvMapRequest, CancellationToken cancellationToken)
    {
        return HandleResult(await _mediator.Send(upsertCsvMapRequest));
    }
    /// <summary>
    /// Submit import 
    /// </summary>
    /// <param name="upsertCsvMapRequest"></param>
    /// <returns></returns>
    [HttpPut("SubmitImport")]
    public async Task<ActionResult<CAResultEmpty>> SubmitImport(SubmitCsvImportRequest submitCsvImportRequest, CancellationToken cancellationToken)
    {
        return HandleResult(await _mediator.Send(submitCsvImportRequest));
    }

    /// <summary>
    /// Test alternative way to handle responses. Using generic response. 
    /// </summary>
    /// <param name="fileData"></param>
    /// <returns></returns>
    [HttpGet("TestAltResponse")]
    public async Task<ActionResult<CAResult<GetCdnTestMethodResponseDto>>> TestAltResponse(GetCdnTestMethodRequest getCdnTestMethodRequest)
    {
        //Test alternative way to handle responses. Using generic response. 
        return HandleResult(await _mediator.Send(getCdnTestMethodRequest));
    }
}
