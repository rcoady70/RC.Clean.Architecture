using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RC.CA.Application.Contracts.Persistence;
using RC.CA.Application.Dto.Cdn;
using RC.CA.Application.Features.Cdn.Queries;
using RC.CA.Application.Features.Club.Queries;
using RC.CA.Infrastructure.Persistence.Cache;

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
        private readonly ICacheProvider _cache;

        public TestController(IMediator mediator,
                              IServiceScopeFactory serviceScopeFactory,
                              ICsvFileRepository csvFileRepository,
                              ICacheProvider cache)
        {
            _mediator = mediator;
            _serviceScopeFactory = serviceScopeFactory;
            _csvFileRepository = csvFileRepository;
            _cache = cache;
        }
        /// <summary>
        /// Test success
        /// </summary>
        /// <returns></returns>
        [HttpGet("TestSuccess")]
        public async Task<ActionResult<CAResult<UpsertCsvMapResponseDto>>> TestSuccess()
        {

            Guid.TryParse("9a6aace3-5c72-467b-a21b-4b0bf8712df3", out Guid id);
            _cache.RemoveFromCache("ssssss");
            var m = await _cache.GetFromCacheAsync<UpsertCsvMapResponseDto>($"{id}");
            if (m == null)
            {
                UpsertCsvMapResponseDto responseDTO = new UpsertCsvMapResponseDto();
                responseDTO.Id = id;
                _cache.AddToCacheAsync<UpsertCsvMapResponseDto>($"{id}", responseDTO);
            }
            m = await _cache.GetFromCacheAsync<UpsertCsvMapResponseDto>($"{id}");
            return HandleResult(CAResultEmpty.Success(m));
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
