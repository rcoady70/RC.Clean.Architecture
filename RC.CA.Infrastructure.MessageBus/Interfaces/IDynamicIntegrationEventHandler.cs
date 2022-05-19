namespace RC.CA.Infrastructure.MessageBus.Interfaces;

public interface IDynamicIntegrationEventHandler
{
    Task Handle(dynamic eventData);
}
