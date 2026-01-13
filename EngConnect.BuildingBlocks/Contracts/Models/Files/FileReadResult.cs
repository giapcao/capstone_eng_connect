namespace EngConnect.BuildingBlock.Contracts.Models.Files;

public class FileReadResult
{
    public required Stream Stream { get; set; }
    public required string FileName { get; set; }
    public required string ContentType { get; set; }
    public long Size { get; set; }
    public required string RelativePath { get; set; }
    public bool IsPrivate { get; set; }
}
