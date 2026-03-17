namespace EngConnect.BuildingBlock.Contracts.Settings;

public sealed class GmailApiSettings
{
    public const string Section = "GmailApiSettings";
    public string ApplicationName { get; set; } = "EngConnect";
    public string ClientId { get; set; } = string.Empty;
    public string ClientSecret { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
    public string SenderEmail { get; set; } = string.Empty;
    public string SenderName { get; set; } = string.Empty;
}
