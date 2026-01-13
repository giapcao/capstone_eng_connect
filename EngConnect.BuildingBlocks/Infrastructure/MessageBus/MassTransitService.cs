using EngConnect.BuildingBlock.Contracts.Abstraction;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace EngConnect.BuildingBlock.Infrastructure.MessageBus;

public class MassTransitService : IMessageBusService
{
    private readonly IBus _bus;
    private readonly ILogger<MassTransitService> _logger;

    public MassTransitService(IBus bus, ILogger<MassTransitService> logger)
    {
        _bus = bus;
        _logger = logger;
    }

    /// <summary>
    ///     Publish a message to the message bus (event)
    /// </summary>
    /// <param name="message"></param>
    /// <param name="cancellationToken"></param>
    /// <typeparam name="T"></typeparam>
    public async Task PublishAsync<T>(T message, CancellationToken cancellationToken = default) where T : class
    {
        _logger.LogInformation("Start publishing message of type {MessageType}", typeof(T).Name);
        await _bus.Publish(message, cancellationToken);
        _logger.LogInformation("End publishing message of type {MessageType}", typeof(T).Name);
    }

    /// <summary>
    ///     Send a message to the message bus (command)
    /// </summary>
    /// <param name="message"></param>
    /// <param name="cancellationToken"></param>
    /// <typeparam name="T"></typeparam>
    public async Task SendAsync<T>(T message, CancellationToken cancellationToken = default) where T : class
    {
        _logger.LogInformation("Start sending message of type {MessageType}", typeof(T).Name);
        await _bus.Send(message, cancellationToken);
        _logger.LogInformation("End sending message of type {MessageType}", typeof(T).Name);
    }

    public async Task PublishManyAsync<T>(IEnumerable<T> messages, CancellationToken cancellationToken = default)
        where T : class
    {
        foreach (var message in messages)
        {
            _logger.LogInformation("Start publishing message of type {MessageType}", typeof(T).Name);
            await _bus.Publish(message, cancellationToken);
            _logger.LogInformation("End publishing message of type {MessageType}", typeof(T).Name);
        }
    }

    public async Task SendManyAsync<T>(IEnumerable<T> messages, CancellationToken cancellationToken = default)
        where T : class
    {
        foreach (var message in messages)
        {
            _logger.LogInformation("Start sending message of type {MessageType}", typeof(T).Name);
            await _bus.Send(message, cancellationToken);
            _logger.LogInformation("End sending message of type {MessageType}", typeof(T).Name);
        }
    }

    public Task PublishAsync(object message, Type eventType, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Start publishing message of type {MessageType}", eventType.Name);
        try
        {
            return _bus.Publish(message, eventType, cancellationToken);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error publishing message of type {MessageType}", eventType.Name);
            throw;
        }
        finally
        {
            _logger.LogInformation("End publishing message of type {MessageType}", eventType.Name);
        }
    }
}