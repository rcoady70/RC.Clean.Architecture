using static RC.CA.Infrastructure.MessageBus.ServiceBusSubscriptionsManager;

namespace RC.CA.Infrastructure.MessageBus.Interfaces;

/// <summary>
/// Manage cross reference between messages and message venet handlers
/// </summary>
public interface IServiceBusSubscriptionsManager
{
    bool IsEmpty { get; }
    event EventHandler<string> OnEventRemoved;
   
    void AddSubscription<T, TH>()
        where T : IntegrationMessage
        where TH : IIntegrationEventHandler<T>;

    void RemoveSubscription<T, TH>()
            where TH : IIntegrationEventHandler<T>
            where T : IntegrationMessage;
 
    bool HasSubscriptionsForEvent<T>() where T : IntegrationMessage;
    bool HasSubscriptionsForEvent(string eventName);
    Type GetEventTypeByName(string eventName);
    void Clear();
    IEnumerable<BusSubscriptionInfo> GetHandlersForEvent<T>() where T : IntegrationMessage;
    IEnumerable<BusSubscriptionInfo> GetHandlersForEvent(string eventName);
    string GetEventKey<T>();
}
