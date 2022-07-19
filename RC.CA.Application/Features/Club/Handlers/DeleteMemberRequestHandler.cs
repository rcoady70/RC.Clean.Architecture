using AutoMapper;
using MediatR;
using RC.CA.Application.Contracts.Persistence;
using RC.CA.Application.Features.Club.Queries;
using RC.CA.Application.Models;

namespace RC.CA.Application.Features.Club.Handlers;
public class DeleteMemberRequestHandler : IRequestHandler<DeleteMemberRequest, CAResult<BaseResponseDto>>
{
    private readonly IMemberRepository _memberRepository;
    private readonly IExperienceRepository _experienceRepository;
    private readonly IMapper _mapper;

    public DeleteMemberRequestHandler(IMemberRepository memberRepository, IExperienceRepository experienceRepository, IMapper mapper)
    {
        _memberRepository = memberRepository;
        _experienceRepository = experienceRepository;
        _mapper = mapper;
    }
    public async Task<CAResult<BaseResponseDto>> Handle(DeleteMemberRequest request, CancellationToken cancellationToken)
    {
        BaseResponseDto baseResponse = new BaseResponseDto();
        var member = await _memberRepository.GetFirstOrDefaultAsync(u => u.Id == request.Id, "Experiences", tracked: true);

        if (member != null)
        {
            await _memberRepository.DeleteAsync(member);
            await _memberRepository.SaveChangesAsync();
            return CAResult<BaseResponseDto>.Success(baseResponse);
        }
        else
            return CAResult<BaseResponseDto>.Invalid("NotFound", $"Record not found for key {request.Id}", ValidationSeverity.Error);
    }
}
