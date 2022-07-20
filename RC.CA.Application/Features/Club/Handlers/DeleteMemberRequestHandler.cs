using AutoMapper;
using MediatR;
using RC.CA.Application.Contracts.Persistence;
using RC.CA.Application.Features.Club.Queries;
using RC.CA.Application.Models;

namespace RC.CA.Application.Features.Club.Handlers;
public class DeleteMemberRequestHandler : IRequestHandler<DeleteMemberRequest, CAResultEmpty>
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
    public async Task<CAResultEmpty> Handle(DeleteMemberRequest request, CancellationToken cancellationToken)
    {
        BaseResponseDto baseResponse = new BaseResponseDto();
        var member = await _memberRepository.GetFirstOrDefaultAsync(u => u.Id == request.Id, "Experiences", tracked: true);

        if (member != null)
        {
            //foreach(var experience in experiences)
            //    await _experienceRepository.DeleteAsync(experience);
            await _memberRepository.DeleteAsync(member);
            await _memberRepository.SaveChangesAsync();
            return CAResultEmpty.Success();
        }
        else
            return CAResultEmpty.Invalid("NotFound", $"Record not found for key {request.Id}", ValidationSeverity.Error);
    }
}
