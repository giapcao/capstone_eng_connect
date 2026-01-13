namespace EngConnect.BuildingBlock.Contracts.Models.Email;

public class EmailContent
{
    public string Subject { get; set; } = null!;
    public string HtmlBody { get; set; } = null!;
    public string? PlainTextBody { get; set; }
    public string? ReplyTo { get; set; }
}