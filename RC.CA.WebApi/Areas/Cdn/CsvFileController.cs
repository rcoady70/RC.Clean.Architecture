﻿using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RC.CA.Application.Contracts.Persistence;
using RC.CA.Application.Dto.Cdn;
using RC.CA.Application.Features.Cdn.Queries;
using RC.CA.Application.Features.Club.Queries;

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
    public async Task<CreateCsvFileResponseDto> Upload([FromForm] IFormFile? fileData)
    {
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
    /// <summary>
    /// Get map based on id
    /// </summary>
    /// <param name="fileData"></param>
    /// <returns></returns>
    [HttpGet("GetMap")]
    public async Task<UpsertCsvMapResponseDto> GetMap(GetCsvMapRequest getCsvMapRequest)
    {
        UpsertCsvMapResponseDto response = new UpsertCsvMapResponseDto();

        GetCsvMapRequestValidator validationRules = new GetCsvMapRequestValidator(_csvFileRepository);
        var valResult = validationRules.Validate(getCsvMapRequest);
        await response.CheckFluentValidationResults(valResult);
    
        if(response.TotalErrors == 0)
            response = await _mediator.Send(getCsvMapRequest);
        else
            return InvalidRequest(response);

        return response;
    }
    /// <summary>
    /// Get csv file list
    /// </summary>
    /// <param name="getCsvFilesListRequest"></param>
    /// <returns></returns>
    [HttpGet("List")]
    public async Task<CsvFilesListResponseDto> List(GetCsvFileListRequest getCsvFilesListRequest)
    {
        CsvFilesListResponseDto response = new CsvFilesListResponseDto();
        response = await _mediator.Send(getCsvFilesListRequest);

        return response;
    }
    /// <summary>
    /// Update map
    /// </summary>
    /// <param name="getCsvMapRequest"></param>
    /// <returns></returns>
    [HttpPatch("UpdateMap")]
    public async Task<UpsertCsvMapResponseDto> UpdateMap(UpsertCsvMapRequest upsertCsvMapRequest)
    {
        UpsertCsvMapResponseDto response = new UpsertCsvMapResponseDto();
        UpsertCsvMapRequestValidator validationRules = new UpsertCsvMapRequestValidator(_csvFileRepository);
        var valResult = validationRules.Validate(upsertCsvMapRequest);
        await response.CheckFluentValidationResults(valResult);
        if (response.TotalErrors == 0)
            response = await _mediator.Send(upsertCsvMapRequest);
        else
            return InvalidRequest(response);
        return response;
    }
    /// <summary>
    /// Submit import 
    /// </summary>
    /// <param name="upsertCsvMapRequest"></param>
    /// <returns></returns>
    [HttpPut("SubmitImport")]
    public async Task<BaseResponseDto> SubmitImport(SubmitCsvImportRequest submitCsvImportRequest)
    {
        BaseResponseDto response = await _mediator.Send(submitCsvImportRequest);
        return response;
    }
}
