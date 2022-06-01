using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using RC.CA.Application.Contracts.Persistence;
using RC.CA.Application.Dto.Cdn;
using RC.CA.Application.Features.Cdn.Queries;
using RC.CA.Application.Models;

namespace RC.CA.Application.Features.Cdn.Handlers
{
    public class UpsertCsvMapRequestHandler : IRequestHandler<UpsertCsvMapRequest, UpsertCsvMapResponseDto>
    {
        private readonly ICsvFileRepository _csvFileRepository;

        public UpsertCsvMapRequestHandler(ICsvFileRepository csvFileRepository)
        {
            _csvFileRepository = csvFileRepository;
        }
        public async Task<UpsertCsvMapResponseDto> Handle(UpsertCsvMapRequest upsertCsvMapRequest, CancellationToken cancellationToken)
        {

            UpsertCsvMapResponseDto response = new UpsertCsvMapResponseDto();
            var csv = await _csvFileRepository.FindAsync(upsertCsvMapRequest.Id,true);
            if (csv != null)
            {
                csv.ColumnMap = upsertCsvMapRequest.ColumnMap.ToJsonExt();
                _csvFileRepository.UpdateAsync(csv);
                _csvFileRepository.SaveChangesAsync();
            }
            return response;
        }
    }
}
