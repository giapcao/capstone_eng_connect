namespace EngConnect.BuildingBlock.Contracts.Settings;

public class FileStorageSettings
{
    public static readonly string Section = "FileStorageSettings";

    // 251221: File management
    public string PrivateFileBasePath = "private/uploads";
    public string PublicFileBasePath = "wwwroot/uploads";

    /// <summary>
    ///     Maximum file size in bytes (default: 10MB)
    /// </summary>
    public long MaxFileSize { get; set; } = 10 * 1024 * 1024;

    /// <summary>
    ///     Allowed file extensions
    /// </summary>
    public readonly List<string> AllowedExtensions = 
    [
        // Image files
        ".jpg", ".jpeg", ".png", ".gif", ".webp",
        // Document files
        ".pdf", ".doc", ".docx", ".txt", ".xls", ".xlsx", 
        // Video files
        ".mp4", ".mov", ".avi",
        // Audio files
        ".mp3", ".wav"
    ];

    /// <summary>
    ///     Base URL for accessing uploaded files
    /// </summary>
    public string BaseUrl { get; set; } = "";

    /// <summary>
    ///     Whether to generate unique filenames to prevent overwriting
    /// </summary>
    public bool UseUniqueFilenames { get; set; } = true;
}