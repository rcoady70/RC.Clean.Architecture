namespace RC.CA.Infrastructure.MessageBus.Interfaces;

public interface IIntegrationEventHandler<in TIntegrationEvent> : IIntegrationEventHandler
    where TIntegrationEvent : IntegrationMessage
{
    Task Handle(TIntegrationEvent eventMessage);
}

public interface IIntegrationEventHandler
{
}
