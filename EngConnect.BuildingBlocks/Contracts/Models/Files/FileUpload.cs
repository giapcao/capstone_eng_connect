namespace EngConnect.BuildingBlock.Contracts.Models.Files;

public class FileUpload
{
    /// <summary>
    ///     Name of the file
    /// </summary>
    public required string FileName { get; set; }

    /// <summary>
    ///     Content type of the file
    /// </summary>
    public required string ContentType { get; set; }

    /// <summary>
    ///     Size of the file in bytes
    /// </summary>
    public long Length { get; set; }

    /// <summary>
    ///     Stream containing the file data
    /// </summary>
    public required Stream Content { get; set; }
} 