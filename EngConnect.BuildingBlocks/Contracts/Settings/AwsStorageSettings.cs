namespace EngConnect.BuildingBlock.Contracts.Settings;

public class AwsStorageSettings
{
    public const string Section = "AwsStorageSettings";

    public string AccessKey { get; set; } = string.Empty;
    public string SecretKey { get; set; } = string.Empty;
    public string Region { get; set; } = string.Empty;
    public string BucketName { get; set; } = string.Empty;
    public string? ServiceUrl { get; set; }
}