using System.Linq.Expressions;
using AutoMapper;
using MediatR;
using RC.CA.Application.Contracts.Persistence;
using RC.CA.Application.Dto;
using RC.CA.Application.Dto.Club;
using RC.CA.Application.Extensions.Linq;
using RC.CA.Application.Features.Club.Queries;
using RC.CA.Domain.Entities.Club;
using RC.CA.SharedKernel.Constants;
using RC.CA.SharedKernel.Extensions;

namespace RC.CA.Application.Features.Club.Handlers;
public class UpdateMemberRequestHandler : IRequestHandler<UpdateMemberRequest, CreateMemberResponseDto>
{
    private readonly IMemberRepository _memberRepository;
    private readonly IExperienceRepository _experienceRepository;
    private readonly IMapper _mapper;

    public UpdateMemberRequestHandler(IMemberRepository memberRepository,
                                      IExperienceRepository experienceRepository, IMapper mapper)
    {
        _memberRepository = memberRepository;
        _experienceRepository = experienceRepository;
        _mapper = mapper;
    }

    public async Task<CreateMemberResponseDto> Handle(UpdateMemberRequest request, CancellationToken cancellationToken)
    {
        CreateMemberResponseDto response = new CreateMemberResponseDto();
        var member = await _memberRepository.GetFirstOrDefaultAsync(m => m.Id == request.Id, "Experiences", tracked: true);
        if (member != null)
        {
            foreach (var item in member.Experiences)
            {
                var exp = request.Experiences.Find(e => e.Id == item.Id);
                if (exp == null)
                    await _experienceRepository.DeleteAsync(item);
            }
            await _experienceRepository.SaveChangesAsync();
            _mapper.Map(request, member);
            int rowsAffected = await _memberRepository.UpdateAsync(member);
            await _memberRepository.SaveChangesAsync();
        }
        return response;
    }
}


