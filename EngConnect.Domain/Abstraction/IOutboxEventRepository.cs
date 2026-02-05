using EngConnect.BuildingBlock.Contracts.Abstraction;
using EngConnect.BuildingBlock.Contracts.Shared;
using EngConnect.Domain.Persistence.Models;

namespace EngConnect.Domain.Abstraction;

public interface IOutboxEventRepository: IGenericRepository<OutboxEvent, Guid>
{
    Task<Result<List<OutboxEvent>>> GetPendingOrRetryableEventsAsync(CancellationToken cancellationToken = default);
    void MarkEventAsPublishedAsync(OutboxEvent @event, CancellationToken cancellationToken = default);
    void MarkEventAsPublishedAsync(List<OutboxEvent> events, CancellationToken cancellationToken = default);

    void MarkEventAsFailedAsync(OutboxEvent @event, int retryCount, DateTime nextRetryAt, string error,
        CancellationToken cancellationToken = default);

    void MarkEventAsDeadAsync(OutboxEvent @event, CancellationToken cancellationToken = default);
}