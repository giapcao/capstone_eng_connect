namespace EngConnect.BuildingBlock.Contracts.Settings;

public class GeminiApiKeySetting
{
    public const string Section = "GeminiSettings";
    public string ApiKey { get; set; } = string.Empty;
    
}