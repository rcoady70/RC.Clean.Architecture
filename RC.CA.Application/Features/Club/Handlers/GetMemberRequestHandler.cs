using System.Linq.Expressions;
using AutoMapper;
using MediatR;
using RC.CA.Application.Contracts.Persistence;
using RC.CA.Application.Dto;
using RC.CA.Application.Dto.Club;
using RC.CA.Application.Extensions.Linq;
using RC.CA.Application.Features.Club.Queries;
using RC.CA.Application.Models;
using RC.CA.Domain.Entities.Club;
using RC.CA.SharedKernel.Constants;
using RC.CA.SharedKernel.Extensions;

namespace RC.CA.Application.Features.Club.Handlers;
public class GetMemberRequestHandler : IRequestHandler<GetMemberRequest, GetMemberResponseDto>
{
    private readonly IMemberRepository _memberRepository;
    private readonly IMapper _mapper;

    public GetMemberRequestHandler(IMemberRepository memberRepository, IMapper mapper)
    {
        _memberRepository = memberRepository;
        _mapper = mapper;
    }

    public async Task<GetMemberResponseDto> Handle(GetMemberRequest request, CancellationToken cancellationToken)
    {
        GetMemberResponseDto response = new GetMemberResponseDto();
        var member = await _memberRepository.GetFirstOrDefaultAsync(m => m.Id == request.Id, "Experiences");
        if (member == null)
            response.AddResponseError(BaseResponseDto.ErrorType.Error, $"Member with id {request.Id} not found");
        else
        {
            response = _mapper.Map<GetMemberResponseDto>(member);
        }
        return response;
    }
}


