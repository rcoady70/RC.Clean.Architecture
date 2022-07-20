using MediatR;
using RC.CA.Application.Contracts.Persistence;
using RC.CA.Application.Dto.Cdn;
using RC.CA.Application.Features.Cdn.Queries;

namespace RC.CA.Application.Features.Cdn.Handlers
{
    public class UpsertCsvMapRequestHandler : IRequestHandler<UpsertCsvMapRequest, CAResult<UpsertCsvMapResponseDto>>
    {
        private readonly ICsvFileRepository _csvFileRepository;

        public UpsertCsvMapRequestHandler(ICsvFileRepository csvFileRepository)
        {
            _csvFileRepository = csvFileRepository;
        }
        public async Task<CAResult<UpsertCsvMapResponseDto>> Handle(UpsertCsvMapRequest upsertCsvMapRequest, CancellationToken cancellationToken)
        {

            UpsertCsvMapResponseDto response = new UpsertCsvMapResponseDto();
            var csv = await _csvFileRepository.FindAsync(upsertCsvMapRequest.Id, true);
            if (csv != null)
            {
                csv.ColumnMap = upsertCsvMapRequest.ColumnMap.ToJsonExt();
                _csvFileRepository.UpdateAsync(csv);
                _csvFileRepository.SaveChangesAsync();
            }
            return CAResult<UpsertCsvMapResponseDto>.Success(response);
        }
    }
}
