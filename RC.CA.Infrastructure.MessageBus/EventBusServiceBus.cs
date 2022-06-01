


using Autofac;
using Microsoft.Extensions.DependencyInjection;

namespace RC.CA.Infrastructure.MessageBus;
/// <summary>
/// Work with service bus, publish mesages, process messages
/// </summary>
public class EventBusServiceBus : IEventBus, IDisposable
{
    private readonly IServiceBusConnection _serviceBusConnection; //Connection to event bus
    private readonly ILogger<EventBusServiceBus> _logger; //logger
    private readonly IServiceBusSubscriptionsManager _subsManager; //Cross reference between events and handlers
    private readonly IServiceScopeFactory _serviceScopeFactory; //Use to create a new scope to control the life time of objects which are in MT environment
    private readonly string _topicOrQueueName = "";
    private readonly string _subscriptionName;
    private ServiceBusSender _sender;
    private ServiceBusProcessor _processor;
    private bool _disposed = false;

    /// <summary>
    /// Event bus 
    /// </summary>
    /// <param name="serviceBusConnection"></param>
    /// <param name="logger"></param>
    /// <param name="subsManager"></param>
    /// <param name="topicOrQueueName"></param>
    /// <param name="subscriptionClientName"></param>
    /// <exception cref="ArgumentNullException"></exception>
    public EventBusServiceBus(IServiceBusConnection serviceBusConnection,
                              ILogger<EventBusServiceBus> logger, 
                              IServiceBusSubscriptionsManager subsManager,
                              IServiceScopeFactory serviceScopeFactory,
                              string topicOrQueueName,
                              string subscriptionClientName)
    {
        _serviceBusConnection = serviceBusConnection;
        _topicOrQueueName = topicOrQueueName;
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _subsManager = subsManager ?? new ServiceBusSubscriptionsManager();
        _subscriptionName = subscriptionClientName;
        _serviceScopeFactory = serviceScopeFactory;
        _sender = _serviceBusConnection.ServiceBusClient.CreateSender(topicOrQueueName);

        ServiceBusProcessorOptions options = new ServiceBusProcessorOptions { MaxConcurrentCalls = 10, AutoCompleteMessages = false };
        _processor = _serviceBusConnection.ServiceBusClient.CreateProcessor(topicOrQueueName, _subscriptionName, options);

        RemoveDefaultRule();
        RegisterSubscriptionClientMessageHandlerAsync().GetAwaiter().GetResult();
    }
    /// <summary>
    /// Publish push message to message bus
    /// </summary>
    /// <param name="eventMessage"></param>
    /// <returns></returns>
    public async Task Publish(IntegrationMessage eventMessage)
    {
        var eventName = eventMessage.GetType().Name;
        var jsonMessage = JsonSerializer.Serialize(eventMessage, eventMessage.GetType());
        var body = Encoding.UTF8.GetBytes(jsonMessage);

        var message = new ServiceBusMessage
        {
            MessageId = Guid.NewGuid().ToString(),
            Body = new BinaryData(body),
            Subject = eventName,
        };

        // Send message to queue
        //
        await _sender.SendMessageAsync(message);
    }
    /// <summary>
    /// Subscribe to subscription
    /// </summary>
    /// <typeparam name="T">Message</typeparam>
    /// <typeparam name="TH">Message handler</typeparam>
    public void Subscribe<T, TH>()  where T : IntegrationMessage
                                    where TH : IIntegrationEventHandler<T>
    {
        var eventName = typeof(T).Name;
        var containsKey = _subsManager.HasSubscriptionsForEvent<T>();
        if (!containsKey)
        {
            try
            {
                //Create filter rule to ensure this message T is only processed by the event handler TH
                //
                _serviceBusConnection.AdministrationClient.CreateRuleAsync(_topicOrQueueName, _subscriptionName, new CreateRuleOptions
                {
                    Filter = new CorrelationRuleFilter() { Subject = eventName },
                    Name = eventName
                }).GetAwaiter().GetResult();
            }
            catch (ServiceBusException ex)
            {
                _logger.LogWarning("The messaging entity {eventName} already exists.", eventName);
            }
        }
        _logger.LogInformation("Subscribing to event {EventName} with {EventHandler}", eventName, typeof(TH).Name);
        
        //Add subscription to in memory dictionary. Link message to message handler
        //
        _subsManager.AddSubscription<T, TH>();
    }
    /// <summary>
    /// Unsubscribe message handler
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TH"></typeparam>
    public void Unsubscribe<T, TH>()
                                    where T : IntegrationMessage
                                    where TH : IIntegrationEventHandler<T>
    {
        var eventName = typeof(T).Name;
        try
        {
            _serviceBusConnection
                .AdministrationClient
                .DeleteRuleAsync(_topicOrQueueName, _subscriptionName, eventName)
                .GetAwaiter()
                .GetResult();
        }
        catch (ServiceBusException ex) when (ex.Reason == ServiceBusFailureReason.MessagingEntityNotFound)
        {
            _logger.LogWarning("The messaging entity {eventName} Could not be found.", eventName);
        }

        _logger.LogInformation("Unsubscribing from event {EventName}", eventName);

        _subsManager.RemoveSubscription<T, TH>();
    }

    
    /// <summary>
    /// Add message event handler to message processor
    /// </summary>
    /// <returns></returns>
    private async Task RegisterSubscriptionClientMessageHandlerAsync()
    {
        _processor.ProcessMessageAsync +=
            async (args) =>
            {
                var eventName = $"{args.Message.Subject}"; 
                string messageData = args.Message.Body.ToString();

                // Process message event 
                //
                if (await ProcessEvent(eventName, messageData))
                {
                    await args.CompleteMessageAsync(args.Message);
                }
            };

        _processor.ProcessErrorAsync += ErrorHandler;
        await _processor.StartProcessingAsync();
    }

    public void Dispose()
    {
        Dispose(true);
    }
    protected virtual void Dispose(bool disposing)
    {
        if (_disposed)
            return;

        if (disposing)
        {
            if (_subsManager!=null)
                _subsManager.Clear();
            if(_processor !=null)
                _processor.CloseAsync().GetAwaiter().GetResult();
            _disposed = true;
        }
    }

    private Task ErrorHandler(ProcessErrorEventArgs args)
    {
        var ex = args.Exception;
        var context = args.ErrorSource;

        _logger.LogError(ex, "Message bus error: {ExceptionMessage} - Context: {@ExceptionContext}", ex.Message, context);

        return Task.CompletedTask;
    }
    /// <summary>
    /// Process message event
    /// </summary>
    /// <param name="eventName"></param>
    /// <param name="message"></param>
    /// <returns></returns>
    private async Task<bool> ProcessEvent(string eventName, string message)
    {
        var processed = false;
        if (_subsManager.HasSubscriptionsForEvent(eventName))
        {
            var subscriptions = _subsManager.GetHandlersForEvent(eventName);
            foreach (var subscription in subscriptions)
            {
                try
                {
                    //Create message handler from event type
                    var eventType = _subsManager.GetEventTypeByName(eventName);
                    var integrationEvent = JsonSerializer.Deserialize(message, eventType);
                    //Create new scope so ALL dependencies are local to this scope. This may be multi threaded so unique scope.
                    using (var scope = _serviceScopeFactory.CreateScope())
                    {
                        //Resolve handler from DI container. This will ensure di references in the constructor will resolve.
                        var instance = scope.ServiceProvider.GetRequiredService(subscription.HandlerType);
                        //Call handle method
                        var concreteType = typeof(IIntegrationEventHandler<>).MakeGenericType(eventType);
                        await (Task)concreteType.GetMethod("Handle").Invoke(instance, new object[] { integrationEvent });
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError($"MessageBus FAILED to process message {eventName} message {message}", ex);
                    throw(ex);
                }
            }
        }
        _logger.LogDebug($"Debug: Completed with status {processed}");
        processed = true;
        return processed;
    }
    /// <summary>
    /// Remove the $Default rile from the subscription. 
    /// Registering creates a rule which matches the message. This is based on the name of the message class.
    ///                                             eventBus.Subscribe<Message, Handler>();
    /// </summary>
    private void RemoveDefaultRule()
    {
        try
        {
            _serviceBusConnection
                .AdministrationClient
                .DeleteRuleAsync(_topicOrQueueName, _subscriptionName, RuleProperties.DefaultRuleName)
                .GetAwaiter()
                .GetResult();
        }
        catch (ServiceBusException ex) when (ex.Reason == ServiceBusFailureReason.MessagingEntityNotFound)
        {
            _logger.LogWarning("The messaging entity {DefaultRuleName} Could not be found.", RuleProperties.DefaultRuleName);
        }
    }
}

public interface IServiceScopedFactory
{
}
