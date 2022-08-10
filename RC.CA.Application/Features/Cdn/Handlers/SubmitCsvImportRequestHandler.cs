using MediatR;
using RC.CA.Application.Contracts.Persistence;
using RC.CA.Application.Features.Cdn.Queries;
using RC.CA.Application.MsgBusHandlers;
using RC.CA.Infrastructure.MessageBus.Interfaces;

namespace RC.CA.Application.Features.Cdn.Handlers
{
    public class SubmitCsvImportRequestHandler : IRequestHandler<SubmitCsvImportRequest, CAResultEmpty>
    {
        private readonly ICsvFileRepository _csvFileRepository;
        private readonly IEventBus _eventBus;

        public SubmitCsvImportRequestHandler(ICsvFileRepository csvFileRepository, IEventBus eventBus)
        {
            _csvFileRepository = csvFileRepository;
            _eventBus = eventBus;
        }
        public async Task<CAResultEmpty> Handle(SubmitCsvImportRequest request, CancellationToken cancellationToken)
        {
            var csvFile = await _csvFileRepository.FindAsync(request.Id, true);
            if (csvFile != null)
            {
                if (csvFile.Status == Domain.Entities.CSV.FileStatus.BeingProcessed)
                    return CAResultEmpty.Invalid("Running", $"Import with id {request.Id} is currently being processed", ValidationSeverity.Error);
                else
                {
                    var ebMessage = new ProcessCsvImportRequestMessage();
                    ebMessage.ImportId = request.Id;
                    _eventBus.Publish(ebMessage);
                    csvFile.Status = Domain.Entities.CSV.FileStatus.OnQueue;
                    csvFile.ProcessedOn = DateTime.MinValue;
                    await _csvFileRepository.UpdateAsync(csvFile);
                    await _csvFileRepository.SaveChangesAsync();
                }
                return CAResultEmpty.Success();
            }
            else
                return CAResultEmpty.Invalid("RecordNotFound", $"Could not find CSV file import with id {request.Id}", ValidationSeverity.Error);
        }
    }
}
