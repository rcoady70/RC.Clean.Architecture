using AutoMapper;
using MediatR;
using RC.CA.Application.Contracts.Persistence;
using RC.CA.Application.Contracts.Services;
using RC.CA.Application.Dto.Cdn;
using RC.CA.Application.Features.Cdn.Queries;

namespace RC.CA.Application.Features.Club.Handlers;

/// <summary>
/// Get csv mapping
/// </summary>
public class GetCsvMapRequestHandler : IRequestHandler<GetCsvMapRequest, CAResult<UpsertCsvMapResponseDto>>
{
    private readonly ICsvMapService _csvMapService;
    private readonly ICsvFileRepository _csvFileRepository;
    private readonly IMapper _mapper;

    public GetCsvMapRequestHandler(ICsvMapService csvMapService, ICsvFileRepository csvFileRepository, IMapper mapper)
    {
        _csvMapService = csvMapService;
        _csvFileRepository = csvFileRepository;
        _mapper = mapper;
    }
    public async Task<CAResult<UpsertCsvMapResponseDto>> Handle(GetCsvMapRequest request, CancellationToken cancellationToken)
    {
        var response = new UpsertCsvMapResponseDto();
        var csvFile = await _csvFileRepository.FindAsync(request.Id);
        if (csvFile != null)
        {
            response.Id = request.Id;
            if (csvFile.ColumnMap.Trim().Length > 0)
                response.ColumnMap = csvFile.ColumnMap.FromJsonExt<List<CsvColumnMapDto>>();
            else
            {
                //Build column map from csv file
                var csvMap = await _csvMapService.BuildMapFromCshHead(request.Id);
                response.ColumnMap = _mapper.Map<List<CsvColumnMapDto>>(csvMap.MapColumns);
            }
        }
        return CAResult<UpsertCsvMapResponseDto>.Success(response);
    }
}
