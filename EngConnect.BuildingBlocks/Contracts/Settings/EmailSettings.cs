namespace EngConnect.BuildingBlock.Contracts.Settings;

public sealed class EmailSettings
{
    public const string Section = "EmailSettings";
    public string SmtpServer { get; set; } = null!;
    public int SmtpPort { get; set; }
    public string SenderEmail { get; set; } = null!;
    public string SenderName { get; set; } = null!;
    public string Password { get; set; } = null!;
    public bool EnableSsl { get; set; }
    public List<string> Cc { get; set; } = [];
}