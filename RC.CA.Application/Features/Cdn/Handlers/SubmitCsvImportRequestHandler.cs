using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using RC.CA.Application.Contracts.Persistence;
using RC.CA.Application.Features.Cdn.Queries;
using RC.CA.Application.Models;
using RC.CA.Application.MsgBusHandlers;
using RC.CA.Infrastructure.MessageBus.Interfaces;

namespace RC.CA.Application.Features.Cdn.Handlers
{
    public class SubmitCsvImportRequestHandler : IRequestHandler<SubmitCsvImportRequest, BaseResponseDto>
    {
        private readonly ICsvFileRepository _csvFileRepository;
        private readonly IEventBus _eventBus;

        public SubmitCsvImportRequestHandler(ICsvFileRepository csvFileRepository, IEventBus eventBus)
        {
            _csvFileRepository = csvFileRepository;
            _eventBus = eventBus;
        }
        public async Task<BaseResponseDto> Handle(SubmitCsvImportRequest request, CancellationToken cancellationToken)
        {
            var response = new BaseResponseDto();
            var csvFile = await _csvFileRepository.FindAsync(request.Id,true);
            if (csvFile != null)
            {
                if(csvFile.Status == Domain.Entities.CSV.FileStatus.BeingProcessed)
                    response.AddResponseError("Running", BaseResponseDto.ErrorType.Error, $"Import with id {request.Id} is currently being processed");
                else
                {
                    var ebMessage = new ProcessCsvImportRequestMessage();
                    ebMessage.ImportId = request.Id;
                    _eventBus.Publish(ebMessage);
                    csvFile.Status = Domain.Entities.CSV.FileStatus.OnQueue;
                    csvFile.ProcessedOn = DateTime.MinValue;
                    _csvFileRepository.UpdateAsync(csvFile);
                    _csvFileRepository.SaveChangesAsync();
                }
            }
            else
                response.AddResponseError("RecordNotFound", BaseResponseDto.ErrorType.Error,$"Could not find CSV file import with id {request.Id}");
            return response;
        }
    }
}
