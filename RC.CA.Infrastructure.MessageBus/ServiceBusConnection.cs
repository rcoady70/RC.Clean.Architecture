﻿namespace RC.CA.Infrastructure.MessageBus;

/// <summary>
/// Connection to service bus, both client and administrator clients
/// </summary>
public class ServiceBusConnection : IServiceBusConnection
{
    private readonly string _serviceBusConnectionString;
    private ServiceBusClient _serviceBusClient;
    private ServiceBusAdministrationClient _subscriptionClient;

    bool _disposed;

    public ServiceBusConnection(string serviceBusConnectionString)
    {
        _serviceBusConnectionString = serviceBusConnectionString;

        //Administrator client
        _subscriptionClient = new ServiceBusAdministrationClient(_serviceBusConnectionString);

        _serviceBusClient = new ServiceBusClient(_serviceBusConnectionString);
    }

    public ServiceBusClient ServiceBusClient
    {
        get
        {
            if (_serviceBusClient.IsClosed)
            {
                _serviceBusClient = new ServiceBusClient(_serviceBusConnectionString);
            }
            return _serviceBusClient;
        }
    }

    public ServiceBusAdministrationClient AdministrationClient
    {
        get
        {
            return _subscriptionClient;
        }
    }

    public ServiceBusClient CreateModel()
    {
        if (_serviceBusClient.IsClosed)
        {
            _serviceBusClient = new ServiceBusClient(_serviceBusConnectionString);
        }

        return _serviceBusClient;
    }

    public void Dispose()
    {
        if (_disposed) return;

        _disposed = true;
        _serviceBusClient.DisposeAsync().GetAwaiter().GetResult();
    }
}
