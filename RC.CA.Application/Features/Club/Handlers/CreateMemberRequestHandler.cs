using AutoMapper;
using MediatR;
using RC.CA.Application.Contracts.Persistence;
using RC.CA.Application.Dto.Club;
using RC.CA.Application.Features.Club.Queries;
using RC.CA.Domain.Entities.Club;

namespace RC.CA.Application.Features.Club.Handlers;
public class CreateMemberRequestHandler : IRequestHandler<CreateMemberRequest, CreateMemberResponseDto>
{
    private readonly IMemberRepository _memberRepository;
    private readonly IMapper _mapper;

    public CreateMemberRequestHandler(IMemberRepository memberRepository, IMapper mapper)
    {
        _memberRepository = memberRepository;
        _mapper = mapper;
    }

    public async Task<CreateMemberResponseDto> Handle(CreateMemberRequest request, CancellationToken cancellationToken)
    {
        Member newMember = new Member(); ;// = _mapper.Map<Member>(request);
        _mapper.Map(request, newMember);
        
        var member = await _memberRepository.AddAsync(newMember);
        await _memberRepository.SaveChangesAsync();
        var response = new CreateMemberResponseDto()
        {
            Id = member.Id
        };
        return response;
    }

}


