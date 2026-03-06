namespace EngConnect.Domain.Abstraction;

public interface IMessageBusWithOutboxService
{
    /// <summary>
    /// Publish a message with fallback to outbox if publishing falls.
    /// If the messsage bus fails, this event is stored in outbox table for later retry.
    /// </summary>
    /// <param name="message"></param>
    /// <param name="aggregateType"></param>
    /// <param name="aggregateId"></param>
    /// <param name="cancellationToken"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    Task PublishWithOutboxFallbackAsync<T>(T message, string aggregateType, Guid aggregateId, CancellationToken cancellationToken = default) where T : class;
}