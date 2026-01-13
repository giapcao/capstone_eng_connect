namespace EngConnect.BuildingBlock.Contracts.Settings;

public class ScheduleJobSettings
{
    public const string Section = "ScheduleJobSettings";

    public bool EnableAllJobs { get; set; }

    public bool EnableOutboxEventJob { get; set; }
}