using System.Linq.Expressions;
using AutoMapper;
using MediatR;
using RC.CA.Application.Contracts.Persistence;
using RC.CA.Application.Dto.Cdn;
using RC.CA.Application.Extensions.Linq;
using RC.CA.Application.Features.Cdn.Queries;
using RC.CA.Domain.Entities.Cdn;

namespace RC.CA.Application.Features.Cdn.Handlers
{
    public class GetCdnFilesListHandler : IRequestHandler<GetCdnFilesListRequest, CAResult<CdnFilesListResponseDto>>
    {
        private readonly ICdnFileRepository _cdnFileRepository;
        private readonly IMapper _mapper;

        public GetCdnFilesListHandler(ICdnFileRepository cdnFileRepository, IMapper mapper)
        {
            _cdnFileRepository = cdnFileRepository;
            _mapper = mapper;
        }


        public async Task<CAResult<CdnFilesListResponseDto>> Handle(GetCdnFilesListRequest request, CancellationToken cancellationToken)
        {
            //Set filter
            Expression<Func<CdnFiles, bool>> filter = null;
            if (!string.IsNullOrEmpty(request.FilterByName))
                filter = LinqExtensionsFilter.BuildPredicate<CdnFiles>("FileName", "contains", request.FilterByName);
            if (!string.IsNullOrEmpty(request.FilterById))
            {
                if (filter == null)
                    filter = LinqExtensionsFilter.BuildPredicate<CdnFiles>("Id", "contains", request.FilterById);
                else
                    filter = filter.And(LinqExtensionsFilter.BuildPredicate<CdnFiles>("Id", "contains", request.FilterById));
            }

            //Set order by from string
            List<SortModel> orderByCollection = null;
            if (!string.IsNullOrEmpty(request.OrderBy))
                orderByCollection = new List<SortModel>() { new SortModel(request.OrderBy) };

            //Execute paginated query
            var cdnFiles = await _cdnFileRepository.GetPagedListAsync(request.PageSeq,
                                                                      request.PageSize,
                                                                      filter,
                                                                      orderByCollection,
                                                                      "");

            //Map response to dto prevent over posting
            var response = new CdnFilesListResponseDto
            {
                Files = _mapper.Map<IReadOnlyList<CdnListDto>>(cdnFiles),
                FilterById = request.FilterById,
                FilterByName = request.FilterByName,
                PaginationMetaData = cdnFiles.PagnationMetaData
            };
            return CAResult<CdnFilesListResponseDto>.Success(response);
        }
    }
}
