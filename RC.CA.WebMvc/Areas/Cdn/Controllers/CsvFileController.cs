using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RC.CA.Application.Contracts.Identity;
using RC.CA.Application.Contracts.Services;
using RC.CA.Application.Dto.Cdn;
using RC.CA.Application.Features.Cdn.Queries;
using RC.CA.WebMvc.Filters.Attributes;

namespace RC.CA.WebMvc.Areas.Cdn.Controllers
{
    [Area("Cdn")]
    public class CsvFileController : RootController
    {
        private readonly IHttpHelper _httpHelper;
        private readonly IMapper _mapper;

        public CsvFileController(IHttpHelper httpHelper, IMapper mapper, IAppContextX appContextX) : base(appContextX)
        {
            _httpHelper = httpHelper;
            _mapper = mapper;
        }
        public IActionResult CsvFileStep1()
        {
            return View();
        }
        [HttpGet]
        [ActionName("CsvFileFinish")]
        public async Task<IActionResult> CsvFileFinish(Guid id)
        {
            if (!ModelState.IsValid) return View();
            ModelState.Clear();
            var response = new UpsertCsvMapRequest()
            {
                Id = id
            };
            return View(response);
        }
        [HttpPost]
        [ActionName("CsvFileFinish")]
        public async Task<IActionResult> CsvFileFinish_Post(Guid id)
        {
            if (!ModelState.IsValid) return View();
            ModelState.Clear();
            var submitCsvImportRequest = new SubmitCsvImportRequest() { Id = id };
            var response = await _httpHelper.SendAsyncCAResult<SubmitCsvImportRequest, BaseResponseCAResult>(submitCsvImportRequest, "api/csvfile/SubmitImport", HttpMethod.Put);

            if (response.IsSuccess)
                return RedirectToAction(nameof(List));
            else
                await AppendErrorsToModelStateAsyncCAResult(response.ValidationErrors);

            var upsertCsvMapRequest = new UpsertCsvMapRequest() { Id = id };
            return View(upsertCsvMapRequest);
        }
        [HttpGet]
        [SaveModelState]
        public async Task<IActionResult> CsvFileStep2(Guid id)
        {
            if (!ModelState.IsValid) return View();
            ModelState.Clear();

            var getCsvMapRequest = new GetCsvMapRequest() { Id = id };
            var response = await _httpHelper.SendAsyncCAResult<GetCsvMapRequest, UpsertCsvMapResponseDto>(getCsvMapRequest, "api/csvfile/getmap", HttpMethod.Get);
            if (response.IsSuccess)
            {
                var vmResponse = _mapper.Map<UpsertCsvMapRequest>(response.Value);
                return View(vmResponse);
            }
            else
                await AppendErrorsToModelStateAsyncCAResult(response.ValidationErrors);

            return RedirectToAction(nameof(List));
        }
        [HttpPost]
        public async Task<IActionResult> CsvFileStep2(UpsertCsvMapRequest upsertCsvMapRequest)
        {
            if (!ModelState.IsValid) return View(upsertCsvMapRequest);
            ModelState.Clear();

            var response = await _httpHelper.SendAsyncCAResult<UpsertCsvMapRequest, UpsertCsvMapResponseDto>(upsertCsvMapRequest, "api/csvfile/updatemap", HttpMethod.Patch);
            if (response.IsSuccess)
                return RedirectToAction(nameof(CsvFileFinish), new { id = upsertCsvMapRequest.Id });
            else
                await AppendErrorsToModelStateAsyncCAResult(response.ValidationErrors);

            return View(upsertCsvMapRequest);
        }
        [HttpGet]
        [RestoreModelState]
        public async Task<IActionResult> List(string? filterByName, string? filterById, string? OrderBy, int? pageSeq)
        {

            GetCsvFileListRequest getCsvFilesListRequest = new GetCsvFileListRequest()
            {
                FilterByName = filterByName ?? "",
                FilterById = filterById ?? "",
                OrderBy = OrderBy ?? "createdon_desc",
                PageSeq = pageSeq ?? 1,
            };

            var response = await _httpHelper.SendAsyncCAResult<GetCsvFileListRequest, CsvFilesListResponseDto>(getCsvFilesListRequest, "api/CsvFile/List", HttpMethod.Get);
            if (response.IsSuccess)
                await AppendErrorsToModelStateAsyncCAResult(response.ValidationErrors);

            //Check if submitted tasks and if so refresh
            if (response.Value.CsvFiles.Where(s => s.Status == Domain.Entities.CSV.FileStatus.BeingProcessed || s.Status == Domain.Entities.CSV.FileStatus.OnQueue).ToList().Count > 0)
                Response.Headers.Add("Refresh", "10");

            return View(response.Value);
        }
    }
}
