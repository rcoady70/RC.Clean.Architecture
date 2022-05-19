namespace RC.CA.Infrastructure.MessageBus;

public interface IServiceBusConnection : IDisposable
{
    ServiceBusClient ServiceBusClient { get; }
    ServiceBusAdministrationClient AdministrationClient { get; }
}
