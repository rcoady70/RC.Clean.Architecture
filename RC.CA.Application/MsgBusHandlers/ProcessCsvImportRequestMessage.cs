using RC.CA.Infrastructure.MessageBus;

namespace RC.CA.Application.MsgBusHandlers;

public class ProcessCsvImportRequestMessage : IntegrationMessage
{
    public Guid ImportId { get; set; }
}
