using Microsoft.Extensions.Logging;
using RC.CA.Application.Contracts.Persistence;
using RC.CA.Infrastructure.MessageBus.Interfaces;

namespace RC.CA.Application.MsgBusHandlers;

public class ProcessCsvImportRequestMessageHandler : IIntegrationEventHandler<ProcessCsvImportRequestMessage>
{
    private readonly ICsvFileRepository _csvFileRepository;
    private readonly ILogger<ProcessCsvImportRequestMessageHandler> _logger;

    public ProcessCsvImportRequestMessageHandler(ICsvFileRepository csvFileRepository,ILogger<ProcessCsvImportRequestMessageHandler> logger)
    {
        _csvFileRepository = csvFileRepository;
        _logger = logger;
    }
    public async Task Handle(ProcessCsvImportRequestMessage eventMessage)
    {
        _logger.LogDebug($"Debug: Starting ProcessCsvImportRequestMessageHandler for message id {eventMessage.ImportId}");
        var csvFile = await _csvFileRepository.FindAsync(eventMessage.ImportId);
        if (csvFile != null)
        {
            csvFile.Status = Domain.Entities.CSV.FileStatus.BeingProcessed;
            await _csvFileRepository.UpdateAsync(csvFile);
            await _csvFileRepository.SaveChangesAsync();

            _logger.LogDebug($"Debug: Running ProcessCsvImportRequestMessageHandler for message id {eventMessage.ImportId}");
            await Task.Delay(10000);
            //To be completed implement actual import from csv file
            csvFile.Status = Domain.Entities.CSV.FileStatus.Finished;
            csvFile.ProcessedOn = DateTime.Now;
            await _csvFileRepository.UpdateAsync(csvFile);
            await _csvFileRepository.SaveChangesAsync();
            _logger.LogDebug($"Debug: Finished ProcessCsvImportRequestMessageHandler for message id {eventMessage.ImportId}");
        }
    }
}
