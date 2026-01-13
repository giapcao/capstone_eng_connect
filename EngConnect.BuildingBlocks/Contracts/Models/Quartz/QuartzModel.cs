namespace EngConnect.BuildingBlock.Contracts.Models.Quartz;

public class QuartzFireAndForgetJobModel
{
    public required string JobName { get; set; }
    public required DateTimeOffset ExecuteAt { get; set; }
}

public class QuartzRecurringCronJobModel
{
    public required string JobName { get; set; }
    public required string CronExpression { get; set; }
}

public class QuartzRecurringIntervalJobModel
{
    public required string JobName { get; set; }
    public required int IntervalInSeconds { get; set; }
    public DateTimeOffset? StartAt { get; set; } // Optional: when to start the recurring job
    public DateTimeOffset? EndAt { get; set; } // Optional: when to end the recurring job
}