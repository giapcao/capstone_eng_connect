namespace EngConnect.BuildingBlock.Contracts.Models.Files;

public class FileEntryResult
{
    public required string RelativePath { get; set; }
    public required string FileName { get; set; }
    public long Size { get; set; }
    public bool IsPrivate { get; set; }
    public DateTimeOffset LastModifiedUtc { get; set; }
}
