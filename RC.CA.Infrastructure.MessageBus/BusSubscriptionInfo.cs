

namespace RC.CA.Infrastructure.MessageBus;

/// <summary>
/// Manage cross reference between message object and its handler
/// </summary>
public partial class ServiceBusSubscriptionsManager : IServiceBusSubscriptionsManager
{
    /// <summary>
    /// Cross reference of message events and handlers
    /// </summary>
    public class BusSubscriptionInfo
    {
        public Type HandlerType { get; }

        private BusSubscriptionInfo(bool isDynamic, Type handlerType)
        {
            HandlerType = handlerType;
        }

        public static BusSubscriptionInfo Typed(Type handlerType) =>  new BusSubscriptionInfo(false, handlerType);
    }
}
