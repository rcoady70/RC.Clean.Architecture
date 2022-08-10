using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RC.CA.Application.Dto.Club;
using RC.CA.Application.Features.Club.Queries;

namespace RC.CA.WebApi.Areas.Club.Controllers;
[Route("api/club/[controller]")]
[ApiController]
public class MembersController : BaseController
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public MembersController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    [HttpGet("Get")]
    public async Task<ActionResult<CAResult<GetMemberResponseDto>>> Get(GetMemberRequest getMemberRequest, CancellationToken cancellationToken)
    {
        return HandleResult(await _mediator.Send(getMemberRequest));
    }
    /// <summary>
    /// Get member list
    /// </summary>
    /// <param name="createMemberRequest"></param>
    /// <returns></returns>
    [HttpGet("List")]
    public async Task<ActionResult<CAResult<MemberListResponseDto>>> List(GetMemberListRequest getMemberListRequest, CancellationToken cancellationToken)
    {
        getMemberListRequest.CacheSkip = true;
        return HandleResult(await _mediator.Send(getMemberListRequest, cancellationToken));
    }
    /// <summary>
    /// Create member with list of experience
    /// </summary>
    /// <param name="createMemberRequest"></param>
    /// <returns></returns>
    [HttpDelete("Delete")]
    public async Task<ActionResult<CAResultEmpty>> Delete(DeleteMemberRequest deleteMemberRequest, CancellationToken cancellationToken)
    {
        return HandleResult(await _mediator.Send(deleteMemberRequest));
    }
    /// <summary>
    /// Create member with list of experience's
    /// </summary>
    /// <param name="createMemberRequest"></param>
    /// <returns></returns>
    [HttpPut("Create")]
    [AllowAnonymous]
    public async Task<ActionResult<CAResult<CreateMemberResponseDto>>> Create(CreateMemberRequest createMemberRequest, CancellationToken cancellationToken)
    {
        return HandleResult(await _mediator.Send(createMemberRequest));
    }
    /// <summary>
    /// Update member with list of experience's
    /// </summary>
    /// <param name="createMemberRequest"></param>
    /// <returns></returns>
    [HttpPatch("Update")]
    public async Task<ActionResult<CAResult<CreateMemberResponseDto>>> Update(UpdateMemberRequest updateMemberRequest, CancellationToken cancellationToken)
    {
        return HandleResult(await _mediator.Send(updateMemberRequest));
    }
}
