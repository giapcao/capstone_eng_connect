namespace EngConnect.BuildingBlock.Contracts.Models.Files;

/// <summary>
///     Result of a file upload operation
/// </summary>
public class FileUploadResult
{
    /// <summary>
    ///     Original filename
    /// </summary>
    public required string OriginalFileName { get; set; }

    /// <summary>
    ///     Stored filename (maybe different if unique names are used)
    /// </summary>
    public required string StoredFileName { get; set; }

    /// <summary>
    ///     File size in bytes
    /// </summary>
    public long Size { get; set; }

    /// <summary>
    ///     Content type of the file
    /// </summary>
    public required string ContentType { get; set; }

    /// <summary>
    ///     Path to the file relative to storage root
    /// </summary>
    public required string RelativePath { get; set; }
    
    /// <summary>
    ///     Path to the file
    /// </summary>
    public string? RelativePathSystem { get; set; }
    
    /// <summary>
    ///     Full URL to access the file
    /// </summary>
    public required string Url { get; set; }
} 