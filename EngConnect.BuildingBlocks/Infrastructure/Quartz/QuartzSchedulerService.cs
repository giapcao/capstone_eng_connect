using System.Net;
using EngConnect.BuildingBlock.Contracts.Abstraction;
using EngConnect.BuildingBlock.Contracts.Models.Quartz;
using EngConnect.BuildingBlock.Contracts.Shared;
using Quartz;

namespace EngConnect.BuildingBlock.Infrastructure.Quartz;

public class QuartzSchedulerService : ISchedulerService
{
    private readonly IScheduler _scheduler;

    public QuartzSchedulerService(IScheduler scheduler)
    {
        _scheduler = scheduler;
    }

    public async Task<Result<ScheduleJobTrackingBase>> CreateFireAndForgetJobAsync<T>(
        QuartzFireAndForgetJobModel fireAndForgetJobModel) where T : IJob
    {
        try
        {
            var jobDetail = JobBuilder.Create<T>().WithIdentity(fireAndForgetJobModel.JobName).Build();
            var trigger = TriggerBuilder.Create()
                .WithIdentity(fireAndForgetJobModel.JobName)
                .StartAt(fireAndForgetJobModel.ExecuteAt)
                .Build();

            await _scheduler.ScheduleJob(jobDetail, trigger);
            return ScheduleJobTrackingBase.Create(fireAndForgetJobModel.JobName, fireAndForgetJobModel.ExecuteAt, null,
                JobType.FireAndForget);
        }
        catch (Exception e)
        {
            return Result.Failure<ScheduleJobTrackingBase>(HttpStatusCode.InternalServerError, 
                new Error(" QuartzSchedulerService.CreateFireAndForgetJobAsync ", "Failed to create fire and forget job: " + e.Message));
        }
    }

    public async Task<Result<ScheduleJobTrackingBase>> CreateRecurringCronJobAsync<T>(
        QuartzRecurringCronJobModel cronJobModel)
        where T : IJob
    {
        try
        {
            var jobDetail = JobBuilder.Create<T>().WithIdentity(cronJobModel.JobName).Build();
            var trigger = TriggerBuilder.Create()
                .WithIdentity(cronJobModel.JobName)
                .WithCronSchedule(cronJobModel.CronExpression)
                .Build();

            var firstFireTime = await _scheduler.ScheduleJob(jobDetail, trigger);
            return ScheduleJobTrackingBase.Create(cronJobModel.JobName, null, firstFireTime, JobType.RecurringCron);
        }
        catch (Exception e)
        {
            return Result.Failure<ScheduleJobTrackingBase>(HttpStatusCode.InternalServerError, 
                new Error(" QuartzSchedulerService.CreateRecurringCronJobAsync ", "Failed to create recurring cron job: " + e.Message));
        }
    }

    public async Task<Result<ScheduleJobTrackingBase>> CreateRecurringIntervalJobAsync<T>(
        QuartzRecurringIntervalJobModel intervalJobModel) where T : IJob
    {
        try
        {
            var jobDetail = JobBuilder.Create<T>().WithIdentity(intervalJobModel.JobName).Build();
            var trigger = TriggerBuilder.Create()
                .WithIdentity(intervalJobModel.JobName)
                .StartAt(intervalJobModel.StartAt ?? DateTimeOffset.UtcNow)
                .EndAt(intervalJobModel.EndAt)
                .WithSimpleSchedule(x => x.WithIntervalInSeconds(intervalJobModel.IntervalInSeconds)
                    .RepeatForever())
                .Build();

            var firstFireTime = await _scheduler.ScheduleJob(jobDetail, trigger);
            return ScheduleJobTrackingBase.Create(intervalJobModel.JobName, null, firstFireTime,
                JobType.RecurringInterval);
        }
        catch (Exception e)
        {
            return Result.Failure<ScheduleJobTrackingBase>(HttpStatusCode.InternalServerError, 
                new Error(" QuartzSchedulerService.CreateRecurringIntervalJobAsync ", "Failed to create recurring interval job: " + e.Message));
        }
    }

    public async Task<bool> RemoveJobAsync(string jobName, CancellationToken cancellationToken)
    {
        var jobKey = new JobKey(jobName);
        if (!await _scheduler.CheckExists(jobKey, cancellationToken)) return false;
        var result = await _scheduler.DeleteJob(jobKey, cancellationToken);
        return result;
    }

    public Task<bool> IsExistAsync(string jobName, CancellationToken cancellationToken)
    {
        var jobKey = new JobKey(jobName);
        return _scheduler.CheckExists(jobKey, cancellationToken);
    }
}