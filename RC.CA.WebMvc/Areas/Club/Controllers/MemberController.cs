using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RC.CA.Application.Contracts.Identity;
using RC.CA.Application.Contracts.Services;
using RC.CA.Application.Dto.Club;
using RC.CA.Application.Features.Club.Queries;

namespace NT.CA.WebUiMvc.Areas.Club.Controllers;
[Area("Club")]
public class MemberController : RootController
{
    private readonly IAppContextX _appContext;
    private readonly IHttpHelper _httpHelper;
    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly IMapper _mapper;

    public MemberController(IAppContextX appContext,
                            IHttpHelper httpHelper,
                            IWebHostEnvironment webHostEnvironment,
                            IMapper mapper) : base(appContext)
    {
        _appContext = appContext;
        _httpHelper = httpHelper;
        _webHostEnvironment = webHostEnvironment;
        _mapper = mapper;
    }
    /// <summary>
    /// Get member list
    /// </summary>
    /// <param name="filterByName"></param>
    /// <param name="filterById"></param>
    /// <param name="OrderBy"></param>
    /// <param name="pageSeq"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<IActionResult> List(string? filterByName, string? filterById, string? OrderBy, int? pageSeq)
    {
        if (!ModelState.IsValid) return View();

        ModelState.Clear();
        GetMemberListRequest getMemberListRequest = new GetMemberListRequest();
        getMemberListRequest.FilterByName = filterByName ?? "";
        getMemberListRequest.FilterById = filterById ?? "";
        getMemberListRequest.OrderBy = OrderBy ?? "createdon_desc";
        getMemberListRequest.PageSeq = pageSeq ?? 1;

        var memberListResponse = await _httpHelper.SendAsync<GetMemberListRequest, MemberListResponseDto>(getMemberListRequest, "api/club/Members/List", HttpMethod.Get);
        if (!memberListResponse.IsSuccess)
            await AppendErrorsToModelStateAsyncCAResult(memberListResponse.ValidationErrors);

        return View(memberListResponse.Value);
    }
    /// <summary>
    /// Delete member. Should be post as get is not secure.
    /// </summary>
    /// <param name="Id"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> Delete(Guid Id)
    {
        if (!ModelState.IsValid) return View();

        var deleteMemberRequest = new DeleteMemberRequest()
        {
            Id = Id
        };
        var memberListResponse = await _httpHelper.SendAsync<DeleteMemberRequest, BaseResponseCAResult>(deleteMemberRequest, "api/club/Members/Delete", HttpMethod.Delete);
        if (!memberListResponse.IsSuccess)
            await AppendErrorsToModelStateAsyncCAResult(memberListResponse.ValidationErrors);
        return RedirectToAction(nameof(List));
    }
    /// <summary>
    /// Add/update member
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<IActionResult> Upsert(Guid? Id)
    {
        if (!ModelState.IsValid) return View();

        var upsertModel = new CreateMemberRequest();
        upsertModel.Experiences.Add(new CreateExperienceRequest());
        //Initialize model with some empty experience requests
        if (Id != null)
        {
            var getMemberRequest = new GetMemberRequest() { Id = Id };
            var member = await _httpHelper.SendAsync<GetMemberRequest, GetMemberResponseDto>(getMemberRequest, "api/club/Members/Get", HttpMethod.Get);
            if (!member.IsSuccess)
                await AppendErrorsToModelStateAsyncCAResult(member.ValidationErrors);
            else
            {
                if (member.Value.Experiences.Count == 0)
                    member.Value.Experiences.Add(new GetMemberExperienceDto());
                upsertModel = _mapper.Map<CreateMemberRequest>(member.Value);
            }
        }
        return View(upsertModel);
    }
    /// <summary>
    /// Update add member
    /// </summary>
    /// <param name="createMemberRequest"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> Upsert(CreateMemberRequest createMemberRequest)
    {
        if (!ModelState.IsValid) return View(createMemberRequest);

        //Save image if one is selected
        if (createMemberRequest.ProfilePhoto != null)
            createMemberRequest.PhotoUrl = await GetUploadedFileName(createMemberRequest);

        //Add member
        if (createMemberRequest.Id == null)
        {
            var memberListResponse = await _httpHelper.SendAsync<CreateMemberRequest, MemberListResponseDto>(createMemberRequest, "api/club/Members/Create", HttpMethod.Put);
            if (!memberListResponse.IsSuccess)
                await AppendErrorsToModelStateAsyncCAResult(memberListResponse.ValidationErrors);
            else
                return RedirectToAction(nameof(List));
        }
        else
        {
            //Update member
            var updateMemberRequest = _mapper.Map<UpdateMemberRequest>(createMemberRequest);
            var memberListResponse = await _httpHelper.SendAsync<UpdateMemberRequest, MemberListResponseDto>(updateMemberRequest, "api/club/Members/Update", HttpMethod.Patch);
            if (!memberListResponse.IsSuccess)
                await AppendErrorsToModelStateAsyncCAResult(memberListResponse.ValidationErrors);
            else
                return RedirectToAction(nameof(List));
        }
        return View(createMemberRequest);
    }
    /// <summary>
    /// Get and save photo uploaded file
    /// </summary>
    /// <param name="createMemberRequest"></param>
    /// <returns></returns>
    private async Task<string> GetUploadedFileName(CreateMemberRequest createMemberRequest)
    {
        string uniqueFileName = "";
        if (createMemberRequest.ProfilePhoto != null)
        {
            string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images");
            uniqueFileName = $"{Guid.NewGuid().ToString()}_{createMemberRequest.ProfilePhoto.FileName}";
            string filePath = Path.Combine(uploadsFolder, uniqueFileName);
            using (FileStream fs = new FileStream(filePath, FileMode.Create))
            {
                await createMemberRequest.ProfilePhoto.CopyToAsync(fs);
            }
        }
        return uniqueFileName;
    }
}
