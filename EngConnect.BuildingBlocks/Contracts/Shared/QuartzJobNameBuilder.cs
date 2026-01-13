using EngConnect.BuildingBlock.Contracts.Models.Quartz;

namespace EngConnect.BuildingBlock.Contracts.Shared;

public class QuartzJobNameBuilder
{
    public static string BuildJobName(Guid entityId, string jobName, JobType jobType)
    {
        var baseJobName = $"{entityId}_{jobName}";
        return jobType switch
        {
            JobType.FireAndForget => $"{baseJobName}-{nameof(JobType.FireAndForget)}",
            JobType.RecurringCron => $"{baseJobName}-{nameof(JobType.RecurringCron)}",
            JobType.RecurringInterval => $"{baseJobName}-{nameof(JobType.RecurringInterval)}",
            _ => throw new ArgumentOutOfRangeException(nameof(jobType), jobType, null)
        };
    }
}