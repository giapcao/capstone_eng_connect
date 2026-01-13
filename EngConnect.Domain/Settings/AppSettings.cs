namespace EngConnect.Domain.Settings;

public class AppSettings
{
    public static readonly string Section = "AppSettings";
    public int OutboxEventIntervalTimeInSeconds { get; set; } = 10;

    public string FrontendUrl { get; set; } = null!;
}