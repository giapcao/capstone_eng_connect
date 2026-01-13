namespace EngConnect.BuildingBlock.Contracts.Abstraction;

public interface IMessageBusService
{
    Task PublishAsync<T>(T message, CancellationToken cancellationToken = default) where T : class;
    Task SendAsync<T>(T message, CancellationToken cancellationToken = default) where T : class;
    Task PublishManyAsync<T>(IEnumerable<T> messages, CancellationToken cancellationToken = default) where T : class;
    Task SendManyAsync<T>(IEnumerable<T> messages, CancellationToken cancellationToken = default) where T : class;
    Task PublishAsync(object message, Type eventType, CancellationToken cancellationToken = default);
}