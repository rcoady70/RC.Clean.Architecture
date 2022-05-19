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
public class GetMemberListRequestHandler : IRequestHandler<GetMemberListRequest, MemberListResponseDto>
{
    private readonly IMemberRepository _memberRepository;
    private readonly IMapper _mapper;

    public GetMemberListRequestHandler(IMemberRepository memberRepository, IMapper mapper)
    {
        _memberRepository = memberRepository;
        _mapper = mapper;
    }

    public async Task<MemberListResponseDto> Handle(GetMemberListRequest request, CancellationToken cancellationToken)
    {
        //Set filter
        Expression<Func<Member, bool>> filter = null;
        if (!string.IsNullOrEmpty(request.FilterByName))
            filter = LinqExtensionsFilter.BuildPredicate<Member>("Name", "contains", request.FilterByName);
        if (!string.IsNullOrEmpty(request.FilterById))
        {
            if (filter == null)
                filter = LinqExtensionsFilter.BuildPredicate<Member>("Id", "contains", request.FilterById);
            else
                filter = filter.And(LinqExtensionsFilter.BuildPredicate<Member>("Id", "contains", request.FilterById));
        }

        //Set order by from string
        List<SortModel> orderByCollection = null;
        if (!string.IsNullOrEmpty(request.OrderBy))
            orderByCollection = new List<SortModel>() { new SortModel(request.OrderBy) };

        //Execute paginated query
        var members = await _memberRepository.GetPagedListAsync(request.PageSeq,
                                                                request.PageSize,
                                                                filter,
                                                                orderByCollection,
                                                                "");

        //Option to enhance return data only selected fields you want to return
        // var xxx = _mapper.Map<IReadOnlyList<MemberListDto>>(members).ShapeData("name,createdon");

        //Map response to dto prevent over posting
        var response = new MemberListResponseDto
        {
            Members = _mapper.Map<IReadOnlyList<MemberListDto>>(members),
            FilterById = request.FilterById,
            FilterByName = request.FilterByName,
            PaginationMetaData = members.PagnationMetaData
        };
        return response;
    }
}


