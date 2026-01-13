using System.Net;
using EngConnect.BuildingBlock.Contracts.Abstraction.Entities;
using EngConnect.BuildingBlock.Contracts.Shared;

namespace EngConnect.BuildingBlock.Contracts.Models.Quartz;

public class ScheduleJobTrackingBase : AuditableEntity<Guid>
{
    public string JobName { get; protected set; } = null!; // Unique name for the job
    public DateTimeOffset? ExecuteAt { get; protected set; } // For one-time jobs
    public DateTimeOffset? LastFireAt { get; protected set; } // For recurring jobs
    public DateTimeOffset? NextFireAt { get; protected set; } // For recurring jobs
    public int RunCount { get; protected set; }
    public bool? IsExecuted { get; protected set; } // Indicates if the job has been executed (for one-time jobs)
    public DateTimeOffset? LastFireFailedAt { get; protected set; }
    public DateTimeOffset? LastFireSucceededAt { get; protected set; }
    public JobType JobType { get; protected set; }

    protected ScheduleJobTrackingBase()
    {
    }

    protected ScheduleJobTrackingBase(string jobName, DateTimeOffset? executeAt, DateTimeOffset? nextFireAt,
        JobType jobType)
    {
        JobName = jobName;
        ExecuteAt = executeAt;
        NextFireAt = nextFireAt;
        JobType = jobType;
    }

    public static Result<ScheduleJobTrackingBase> Create(string jobName, DateTimeOffset? executeAt,
        DateTimeOffset? nextFireAt, JobType jobType)
    {
        var validationResult = ValidateBaseProperties(jobName, executeAt, nextFireAt, jobType);
        if (validationResult.IsFailure)
        {
            return Result<ScheduleJobTrackingBase>.FromResult(validationResult);
        }

        return jobType switch
        {
            JobType.FireAndForget => new ScheduleJobTrackingBase(jobName, executeAt, null, jobType),
            JobType.RecurringCron => new ScheduleJobTrackingBase(jobName, null, nextFireAt, jobType),
            JobType.RecurringInterval => new ScheduleJobTrackingBase(jobName, null, nextFireAt, jobType),
            _ => throw new ArgumentOutOfRangeException(nameof(jobType), jobType, null)
        };
    }

    private static Result ValidateBaseProperties(string jobName, DateTimeOffset? executeAt,
        DateTimeOffset? nextFireAt, JobType jobType)
    {
        if (string.IsNullOrWhiteSpace(jobName))
        {
            return Result.Failure<ScheduleJobTrackingBase>(HttpStatusCode.BadRequest,
                new Error("InvalidParameters", "Tên công việc không được để trống."));
        }

        if (jobType == JobType.FireAndForget)
        {
            if (executeAt == null)
            {
                return Result.Failure<ScheduleJobTrackingBase>(HttpStatusCode.BadRequest,
                    new Error("InvalidParameters",
                        "Thời gian thực thi không được để trống đối với công việc thuộc loại FireAndForget."));
            }

            if (nextFireAt != null)
            {
                return Result.Failure<ScheduleJobTrackingBase>(HttpStatusCode.BadRequest,
                    new Error("InvalidParameters",
                        "Lần thực thi tiếp theo phải để trống đối với công việc thuộc loại FireAndForget."));
            }
        }

        if (jobType is JobType.RecurringCron or JobType.RecurringInterval)
        {
            if (nextFireAt == null)
            {
                return Result.Failure<ScheduleJobTrackingBase>(HttpStatusCode.BadRequest,
                    new Error("InvalidParameters",
                        "Lần thực thi tiếp theo không được để trống đối với công việc thuộc loại Recurring."));
            }

            if (executeAt != null)
            {
                return Result.Failure<ScheduleJobTrackingBase>(HttpStatusCode.BadRequest,
                    new Error("InvalidParameters",
                        "Thời gian thực thi phải để trống đối với công việc thuộc loại Recurring."));
            }
        }

        return Result.Success();
    }

    public void MarkAsExecuted()
    {
        IsExecuted = JobType == JobType.FireAndForget ? true : null;
    }

    public void UpdateFireTimes(DateTimeOffset? lastFireAt, DateTimeOffset? nextFireAt, bool isSuccess)
    {
        LastFireAt = lastFireAt;
        NextFireAt = nextFireAt;

        if (isSuccess)
        {
            LastFireSucceededAt = lastFireAt;
            LastFireFailedAt = null;
        }
        else
        {
            LastFireFailedAt = lastFireAt;
        }
    }

    public void IncrementRunCount()
    {
        RunCount += 1;
    }
}