using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RC.CA.Application.Contracts.Persistence;
using RC.CA.Application.Dto.Cdn;
using RC.CA.Application.Features.Cdn.Queries;
using RC.CA.Application.Features.Club.Queries;

namespace RC.CA.WebApi.Areas.Cdn
{
    /// <summary>
    /// Used for generic testing
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class TestController : BaseController
    {
        private readonly IMediator _mediator;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly ICsvFileRepository _csvFileRepository;

        public TestController(IMediator mediator, IServiceScopeFactory serviceScopeFactory, ICsvFileRepository csvFileRepository)
        {
            _mediator = mediator;
            _serviceScopeFactory = serviceScopeFactory;
            _csvFileRepository = csvFileRepository;
        }
        /// <summary>
        /// Test success
        /// </summary>
        /// <returns></returns>
        [HttpGet("TestSuccess")]
        public async Task<ActionResult<CAResult<UpsertCsvMapResponseDto>>> TestSuccess()
        {
            UpsertCsvMapResponseDto responseDTO = new UpsertCsvMapResponseDto();
            return HandleResult(CAResultEmpty.Success(responseDTO));
        }

        /// <summary>
        /// Test fluent validation errors 
        /// </summary>
        /// <returns></returns>
        [HttpGet("TestFluentErrors")]
        public async Task<ActionResult<CAResult<UpsertCsvMapResponseDto>>> TestFluentErrors()
        {
            //Fluent validation 
            UpsertCsvMapResponseDto responseDTO = new UpsertCsvMapResponseDto();
            GetCdnFilesRequestValidator validationRules = new GetCdnFilesRequestValidator();

            var valResult = validationRules.Validate(new GetCdnFilesListRequest() { FilterByName = "<" });
            if (!valResult.IsValid)
                return HandleResult(CAResult<UpsertCsvMapResponseDto>.Invalid(valResult.AsModelStateErrors()));
            else
                return HandleResult(CAResult<UpsertCsvMapResponseDto>.Success(responseDTO, "Ran ok no errors"));
        }

        /// <summary>
        /// Test exception in controller
        /// </summary>
        /// <returns></returns>
        [HttpGet("TestExceptionController")]
        public async Task TestExceptionController()
        {
            throw (new NotImplementedException("Test exception in controller"));
        }

        /// <summary>
        /// Test exception in controller
        /// </summary>
        /// <returns></returns>
        [HttpGet("TestNotFound")]
        public async Task<ActionResult<CAResult<UpsertCsvMapResponseDto>>> TestNotFound()
        {
            return HandleResult(CAResult<UpsertCsvMapResponseDto>.NotFound());

        }

    }
}
