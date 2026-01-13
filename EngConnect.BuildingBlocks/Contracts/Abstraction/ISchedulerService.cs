using EngConnect.BuildingBlock.Contracts.Models.Quartz;
using EngConnect.BuildingBlock.Contracts.Shared;
using Quartz;

namespace EngConnect.BuildingBlock.Contracts.Abstraction;

public interface ISchedulerService
{
    Task<Result<ScheduleJobTrackingBase>> CreateFireAndForgetJobAsync<T>(QuartzFireAndForgetJobModel fireAndForgetJobModel)
        where T : IJob;

    Task<Result<ScheduleJobTrackingBase>> CreateRecurringCronJobAsync<T>(QuartzRecurringCronJobModel cronJobModel)
        where T : IJob;

    Task<Result<ScheduleJobTrackingBase>> CreateRecurringIntervalJobAsync<T>(
        QuartzRecurringIntervalJobModel intervalJobModel) where T : IJob;

    /// <summary>
    ///     Return true if job is removed successfully, false if job does not exist or failed to remove
    /// </summary>
    /// <param name="jobName"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<bool> RemoveJobAsync(string jobName, CancellationToken cancellationToken);

    Task<bool> IsExistAsync(string jobName, CancellationToken cancellationToken);
}