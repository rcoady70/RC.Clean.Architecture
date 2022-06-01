using System.Net;
using Microsoft.AspNetCore.Mvc;
using RC.CA.Application.Contracts.Identity;
using RC.CA.Application.Contracts.Services;
using RC.CA.Application.Dto.Cdn;
using RC.CA.Application.Features.Cdn.Queries;
using Microsoft.AspNetCore.Authorization;

namespace RC.CA.WebMvc.Areas.Cdn.Controllers
{
    [Area("Cdn")]
    public class ImageController :  RootController
    {
        private readonly IHttpHelper _httpHelper;

        public ImageController(IHttpHelper httpHelper, IAppContextX appContextX) : base(appContextX)
        {
            _httpHelper = httpHelper;
        }
        public async Task<IActionResult> List(string? filterByName, string? filterById, string? OrderBy, int? pageSeq)
        {
            if (!ModelState.IsValid) return View();

            ModelState.Clear();
            GetCdnFilesListRequest getCdnFilesListRequest = new GetCdnFilesListRequest();
            getCdnFilesListRequest.FilterByName = filterByName ?? "";
            getCdnFilesListRequest.FilterById = filterById ?? "";
            getCdnFilesListRequest.OrderBy = OrderBy ?? "createdon_desc";
            getCdnFilesListRequest.PageSeq = pageSeq ?? 1;

            var memberListResponse = await _httpHelper.SendAsync<GetCdnFilesListRequest, CdnFilesListResponseDto>(getCdnFilesListRequest, "api/cdn/image/list", HttpMethod.Get);
            if (memberListResponse?.TotalErrors > 0)
            {
                if ((HttpStatusCode)memberListResponse.RequestStatus == HttpStatusCode.Unauthorized)
                    return RedirectToAction("Login", "UserAccount", new { area = "account" });
                await AppendErrorsToModelStateAsync(memberListResponse);
            }
            return View(memberListResponse);
        }
        /// <summary>
        /// Called to refresh image list through ajax call.
        /// </summary>
        /// <param name="filterByName"></param>
        /// <param name="filterById"></param>
        /// <param name="OrderBy"></param>
        /// <param name="pageSeq"></param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ImageListRefresh(string? filterByName, string? filterById, string? OrderBy, int? pageSeq)
        {
            if (!ModelState.IsValid) return View();

            ModelState.Clear();
            GetCdnFilesListRequest getCdnFilesListRequest = new GetCdnFilesListRequest();
            getCdnFilesListRequest.FilterByName = filterByName ?? "";
            getCdnFilesListRequest.FilterById = filterById ?? "";
            getCdnFilesListRequest.OrderBy = OrderBy ?? "createdon_desc";
            getCdnFilesListRequest.PageSeq = pageSeq ?? 1;

            var memberListResponse = await _httpHelper.SendAsync<GetCdnFilesListRequest, CdnFilesListResponseDto>(getCdnFilesListRequest, "api/cdn/image/list", HttpMethod.Get);
            if (memberListResponse?.TotalErrors > 0)
            {
                if ((HttpStatusCode)memberListResponse.RequestStatus == HttpStatusCode.Unauthorized)
                    return RedirectToAction("Login", "UserAccount", new { area = "account" });
                await AppendErrorsToModelStateAsync(memberListResponse);
            }
            return PartialView("_ImageList", memberListResponse);
        }
        /// <summary>
        /// Delete image. Should be post as get is not secure.
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Delete(Guid Id)
        {
            if (!ModelState.IsValid) return View();

            var deleteMemberRequest = new DeleteCdnFileRequest()
            {
                Id = Id
            };
            var cdnFileListResponse = await _httpHelper.SendAsync<DeleteCdnFileRequest, BaseResponseDto>(deleteMemberRequest, "api/cdn/image/Delete",HttpMethod.Delete);
            if (cdnFileListResponse?.TotalErrors > 0)
                await AppendErrorsToModelStateAsync(cdnFileListResponse);
            return RedirectToAction(nameof(List));
        }
    }
}
