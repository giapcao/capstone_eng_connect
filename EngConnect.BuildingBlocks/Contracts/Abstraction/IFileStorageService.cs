using EngConnect.BuildingBlock.Contracts.Models.Files;
using EngConnect.BuildingBlock.Contracts.Shared;

namespace EngConnect.BuildingBlock.Contracts.Abstraction;

/// <summary>
///     Service for handling file storage operations
/// </summary>
public interface IFileStorageService
{
    /// <summary>
    ///     Upload a file to storage under the selected visibility scope.
    /// </summary>
    /// <param name="file">The file to upload</param>
    /// <param name="isPrivate">Flag indicating whether to use the private or public base path</param>
    /// <param name="directory">Optional subdirectory within the selected base path</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Result containing file information if successful</returns>
    Task<Result<FileUploadResult>> UploadFileAsync(FileUpload file, bool isPrivate, string? directory = null, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Get file content and metadata.
    /// </summary>
    /// <param name="relativePath">Path to the file relative to the selected base path</param>
    /// <param name="isPrivate">Flag indicating whether to read from the private or public base path</param>
    Result<FileReadResult> GetFile(string relativePath, bool isPrivate);

    /// <summary>
    ///     List files under a directory for the selected scope.
    /// </summary>
    /// <param name="isPrivate">Flag indicating whether to read from the private or public base path</param>
    /// <param name="directory">Optional subdirectory to scope the listing</param>
    /// <param name="cancellationToken">Cancellation token</param>
    Result<List<FileEntryResult>> ListFiles(bool isPrivate, string? directory = null, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Delete a file from storage.
    /// </summary>
    /// <param name="relativePath">Path to the file relative to the selected base path</param>
    /// <param name="isPrivate">Flag indicating whether to delete from the private or public base path</param>
    Result DeleteFile(string relativePath, bool isPrivate);

    /// <summary>
    ///     Move a file between public/private roots. The flag denotes the current scope; the file is moved to the opposite scope.
    /// </summary>
    /// <param name="relativePath">Path to the file relative to its current base path</param>
    /// <param name="sourceIsPrivate">Flag indicating the file currently resides in the private path; if false, file is expected in public path</param>
    Result<FileUploadResult> MoveFile(string relativePath, bool sourceIsPrivate);

    /// <summary>
    ///     Validate if a file is allowed based on size and extension
    /// </summary>
    /// <param name="file">The file to validate</param>
    /// <returns>Result indicating if file is valid</returns>
    Result ValidateFile(FileUpload file);
} 