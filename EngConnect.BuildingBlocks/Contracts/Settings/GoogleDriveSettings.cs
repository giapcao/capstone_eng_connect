namespace EngConnect.BuildingBlock.Contracts.Settings;

public class GoogleDriveSettings
{
    public const string Section = "GoogleDrive";
    public string ApplicationName { get; set; } = "My App";
    public string ClientId { get; set; } = string.Empty;
    public string ClientSecret { get; set; } =string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
    public string MainFolderId { get; set; } =  string.Empty; 
}