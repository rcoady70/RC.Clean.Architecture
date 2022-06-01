using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using RC.CA.Application.Contracts.Persistence;
using RC.CA.Application.Dto.Cdn;
using RC.CA.Application.Extensions.Linq;
using RC.CA.Application.Features.Cdn.Queries;
using RC.CA.Domain.Entities.Cdn;
using RC.CA.Domain.Entities.CSV;

namespace RC.CA.Application.Features.Cdn.Handlers
{
    public class GetCsvFileListHandler : IRequestHandler<GetCsvFileListRequest, CsvFilesListResponseDto>
    {
        private readonly ICsvFileRepository _csvFileRepository;
        private readonly IMapper _mapper;

        public GetCsvFileListHandler(ICsvFileRepository csvFileRepository, IMapper mapper)
        {
            _csvFileRepository = csvFileRepository;
            _mapper = mapper;
        }


        public async Task<CsvFilesListResponseDto> Handle(GetCsvFileListRequest request, CancellationToken cancellationToken)
        {
            //Set filter
            Expression<Func<CsvFile, bool>> filter = null;
            if (!string.IsNullOrEmpty(request.FilterByName))
                filter = LinqExtensionsFilter.BuildPredicate<CsvFile>("FileName", "contains", request.FilterByName);
            if (!string.IsNullOrEmpty(request.FilterById))
            {
                if (filter == null)
                    filter = LinqExtensionsFilter.BuildPredicate<CsvFile>("Id", "contains", request.FilterById);
                else
                    filter = filter.And(LinqExtensionsFilter.BuildPredicate<CsvFile>("Id", "contains", request.FilterById));
            }

            //Set order by from string
            List<SortModel> orderByCollection = null;
            if (!string.IsNullOrEmpty(request.OrderBy))
                orderByCollection = new List<SortModel>() { new SortModel(request.OrderBy) };

            //Execute paginated query
            var csvFiles = await _csvFileRepository.GetPagedListAsync(request.PageSeq,
                                                                      request.PageSize,
                                                                      filter,
                                                                      orderByCollection,
                                                                      "");

            //Map response to dto prevent over posting
            var response = new CsvFilesListResponseDto
            {
                CsvFiles = _mapper.Map<IReadOnlyList<CsvFileListDto>>(csvFiles),
                FilterById = request.FilterById,
                FilterByName = request.FilterByName,
                PaginationMetaData = csvFiles.PagnationMetaData
            };
            return response;
        }
    }
}
