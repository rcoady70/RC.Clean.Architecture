using AutoMapper;
using MediatR;
using RC.CA.Application.Contracts.Persistence;
using RC.CA.Application.Dto.Club;
using RC.CA.Application.Features.Club.Queries;

namespace RC.CA.Application.Features.Club.Handlers;
public class GetMemberRequestHandler : IRequestHandler<GetMemberRequest, CAResult<GetMemberResponseDto>>
{
    private readonly IMemberRepository _memberRepository;
    private readonly IMapper _mapper;

    public GetMemberRequestHandler(IMemberRepository memberRepository, IMapper mapper)
    {
        _memberRepository = memberRepository;
        _mapper = mapper;
    }

    public async Task<CAResult<GetMemberResponseDto>> Handle(GetMemberRequest request, CancellationToken cancellationToken)
    {
        var member = await _memberRepository.GetFirstOrDefaultAsync(m => m.Id == request.Id, "Experiences");
        if (member == null)
        {
            var invalidResp = CAResult<GetMemberResponseDto>.Invalid(
                new ValidationError
                {
                    ErrorCode = "MRH00001",
                    ErrorMessage = $"Member with id {request.Id} not found",
                    Identifier = "",
                    Severity = ValidationSeverity.Error
                });
            return invalidResp;
        }
        else
            return CAResult<GetMemberResponseDto>.Success(_mapper.Map<GetMemberResponseDto>(member));
    }
}


