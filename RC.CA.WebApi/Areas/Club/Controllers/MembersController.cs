using AutoMapper;
using MediatR;
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
    public async Task<CAResult<GetMemberResponseDto>> Get(GetMemberRequest getMemberRequest)
    {
        var memberResponse = await _mediator.Send(getMemberRequest);
        return memberResponse;
    }
    /// <summary>
    /// Get member list
    /// </summary>
    /// <param name="createMemberRequest"></param>
    /// <returns></returns>
    [HttpGet("List")]
    public async Task<MemberListResponseDto> List(GetMemberListRequest getMemberListRequest)
    {
        Request.Headers.TryGetValue(WebConstants.CorrelationId, out var correlationId);

        MemberListResponseDto memberResponse = new MemberListResponseDto();
        GetMemberListRequestValidator validationRules = new GetMemberListRequestValidator();
        var valResult = validationRules.Validate(getMemberListRequest);
        memberResponse.CheckFluentValidationResults(valResult);

        //Check result of model validation
        if (memberResponse.TotalErrors == 0)
            memberResponse = await _mediator.Send(getMemberListRequest);
        else
            return InvalidRequest(memberResponse);

        return memberResponse;
    }
    /// <summary>
    /// Create member with list of experience
    /// </summary>
    /// <param name="createMemberRequest"></param>
    /// <returns></returns>
    [HttpDelete("Delete")]
    public async Task<BaseResponseDto> Delete(DeleteMemberRequest deleteMemberRequest)
    {
        BaseResponseDto memberResponse = new BaseResponseDto();

        CreateExpierenceRequestValidator validationRulesExp = new CreateExpierenceRequestValidator();
        memberResponse = await _mediator.Send(deleteMemberRequest);

        return memberResponse;
    }
    /// <summary>
    /// Create member with list of experience's
    /// </summary>
    /// <param name="createMemberRequest"></param>
    /// <returns></returns>
    [HttpPut("Create")]
    public async Task<CreateMemberResponseDto> Create(CreateMemberRequest createMemberRequest)
    {
        CreateMemberResponseDto memberResponse = new CreateMemberResponseDto();
        CreateMemberRequestValidator validationRules = new CreateMemberRequestValidator();
        var valResult = validationRules.Validate(createMemberRequest);
        await memberResponse.CheckFluentValidationResults(valResult);

        //Validate experience records
        CreateExpierenceRequestValidator validationRulesExp = new CreateExpierenceRequestValidator();
        foreach (var item in createMemberRequest.Experiences)
        {
            valResult = validationRulesExp.Validate(item);
            await memberResponse.CheckFluentValidationResults(valResult);
        }
        //
        if (memberResponse.TotalErrors == 0)
        {
            memberResponse = await _mediator.Send(createMemberRequest);
        }
        else
            return InvalidRequest(memberResponse);

        return memberResponse;
    }
    /// <summary>
    /// Update member with list of experience's
    /// </summary>
    /// <param name="createMemberRequest"></param>
    /// <returns></returns>
    [HttpPatch("Update")]
    public async Task<CreateMemberResponseDto> Update(UpdateMemberRequest updateMemberRequest)
    {
        CreateMemberResponseDto memberResponse = new CreateMemberResponseDto();
        UpdateMemberRequestValidator validationRules = new UpdateMemberRequestValidator();
        var valResult = validationRules.Validate(updateMemberRequest);
        await memberResponse.CheckFluentValidationResults(valResult);

        //Validate experience records
        CreateExpierenceRequestValidator validationRulesExp = new CreateExpierenceRequestValidator();
        foreach (var item in updateMemberRequest.Experiences)
        {
            valResult = validationRulesExp.Validate(item);
            await memberResponse.CheckFluentValidationResults(valResult);
        }
        //
        if (memberResponse.TotalErrors == 0)
        {
            memberResponse = await _mediator.Send(updateMemberRequest);
        }
        else
            return InvalidRequest(memberResponse);

        return memberResponse;
    }

}
