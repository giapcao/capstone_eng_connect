namespace EngConnect.Domain.Settings;

public class AppSettings
{
    public static readonly string Section = "AppSettings";
    public int OutboxEventIntervalTimeInSeconds { get; set; } = 10;

    public string FrontendUrl { get; set; } = null!;
    
    //260307: Add ForwardHeader and HttpDirection Flag
    public bool EnableForwardedHeaders { get; set; }
    public bool EnableHttpsRedirection { get; set; }
    
    //260321: Add default avatar path
    public string DefaultAvatarPath { get; set; } = null!;
}