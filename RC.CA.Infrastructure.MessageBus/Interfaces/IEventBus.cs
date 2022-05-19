
namespace RC.CA.Infrastructure.MessageBus.Interfaces;
/// <summary>
/// Work with service bus, publish messages, process messages, subscribe to events
/// </summary>
public interface IEventBus
{
    /// <summary>
    /// Publish message to event bus
    /// </summary>
    /// <param name="eventMessage"></param>
    /// <returns></returns>
    Task Publish(IntegrationMessage eventMessage);
    /// <summary>
    /// Attach message handler
    /// </summary>
    /// <typeparam name="M">Message</typeparam>
    /// <typeparam name="MH">Message handler</typeparam>
    void Subscribe<M, MH>()
                            where M : IntegrationMessage
                            where MH : IIntegrationEventHandler<M>;
    /// <summary>
    /// Detach message handler
    /// </summary>
    /// <typeparam name="M">Message</typeparam>
    /// <typeparam name="MH">Message handler</typeparam>
    void Unsubscribe<M, MH>()
                                where MH : IIntegrationEventHandler<M>
                                where M : IntegrationMessage;
}
