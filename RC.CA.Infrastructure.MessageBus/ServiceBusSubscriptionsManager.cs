namespace RC.CA.Infrastructure.MessageBus;
/// <summary>
/// Manage cross reference between event bus messages and there handlers
/// </summary>
public partial class ServiceBusSubscriptionsManager : IServiceBusSubscriptionsManager
{
    private readonly Dictionary<string, List<BusSubscriptionInfo>> _handlers;
    private readonly List<Type> _eventTypes;
    public event EventHandler<string> OnEventRemoved;

    public ServiceBusSubscriptionsManager()
    {
        _handlers = new Dictionary<string, List<BusSubscriptionInfo>>();
        _eventTypes = new List<Type>();
    }

    public bool IsEmpty => _handlers is { Count: 0 };
    public void Clear() => _handlers.Clear();

    /// <summary>
    /// Add subscription message T / handler TH to cross reference
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TH"></typeparam>

    public void AddSubscription<T, TH>()
        where T : IntegrationMessage
        where TH : IIntegrationEventHandler<T>
    {
        var eventName = GetEventKey<T>();

        AddHandler(typeof(TH), eventName);

        if (!_eventTypes.Contains(typeof(T)))
        {
            _eventTypes.Add(typeof(T));
        }
    }
    /// <summary>
    /// Add subscription message / handler to cross reference
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TH"></typeparam>
    private void AddHandler(Type handlerType, string eventName)
    {
        if (!HasSubscriptionsForEvent(eventName))
        {
            _handlers.Add(eventName, new List<BusSubscriptionInfo>());
        }

        if (_handlers[eventName].Any(s => s.HandlerType == handlerType))
        {
            throw new ArgumentException(
                $"Handler Type {handlerType.Name} already registered for '{eventName}'", nameof(handlerType));
        }
       
        _handlers[eventName].Add(BusSubscriptionInfo.Typed(handlerType));
    }
    /// <summary>
    /// Remove subscription message T / handler TH to cross reference
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TH"></typeparam>
    public void RemoveSubscription<T, TH>()
        where TH : IIntegrationEventHandler<T>
        where T : IntegrationMessage
    {
        var handlerToRemove = FindSubscriptionToRemove<T, TH>();
        var eventName = GetEventKey<T>();
        RemoveHandler(eventName, handlerToRemove);
    }
    /// <summary>
    /// Remove event handler
    /// </summary>
    /// <param name="eventName"></param>
    /// <param name="subsToRemove"></param>
    private void RemoveHandler(string eventName, BusSubscriptionInfo subsToRemove)
    {
        if (subsToRemove != null)
        {
            _handlers[eventName].Remove(subsToRemove);
            if (!_handlers[eventName].Any())
            {
                _handlers.Remove(eventName);
                var eventType = _eventTypes.SingleOrDefault(e => e.Name == eventName);
                if (eventType != null)
                {
                    _eventTypes.Remove(eventType);
                }
                RaiseOnEventRemoved(eventName);
            }
        }
    }

    public IEnumerable<BusSubscriptionInfo> GetHandlersForEvent<T>() where T : IntegrationMessage
    {
        var key = GetEventKey<T>();
        return GetHandlersForEvent(key);
    }
    public IEnumerable<BusSubscriptionInfo> GetHandlersForEvent(string eventName) => _handlers[eventName];

    private void RaiseOnEventRemoved(string eventName)
    {
        var handler = OnEventRemoved;
        handler?.Invoke(this, eventName);
    }

    private BusSubscriptionInfo FindSubscriptionToRemove<T, TH>()
            where T : IntegrationMessage
            where TH : IIntegrationEventHandler<T>
    {
        var eventName = GetEventKey<T>();
        return DoFindSubscriptionToRemove(eventName, typeof(TH));
    }

    private BusSubscriptionInfo DoFindSubscriptionToRemove(string eventName, Type handlerType)
    {
        if (!HasSubscriptionsForEvent(eventName))
        {
            return null;
        }

        return _handlers[eventName].SingleOrDefault(s => s.HandlerType == handlerType);
    }

    public bool HasSubscriptionsForEvent<T>() where T : IntegrationMessage
    {
        var key = GetEventKey<T>();
        return HasSubscriptionsForEvent(key);
    }
    public bool HasSubscriptionsForEvent(string eventHandlerName) => _handlers.ContainsKey(eventHandlerName);

    public Type GetEventTypeByName(string eventHandlerName) => _eventTypes.SingleOrDefault(t => t.Name == eventHandlerName);

    public string GetEventKey<T>()
    {
        return typeof(T).Name;
    }
}
