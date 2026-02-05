using EngConnect.BuildingBlock.Contracts.Shared;

namespace EngConnect.Application.Abstraction;

public interface IOutboxEventScheduler
{
    Task<Result> ScheduleOutboxEventAsync(CancellationToken cancellationToken = default);
}