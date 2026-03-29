using EngConnect.BuildingBlock.Contracts.Models.Files;

namespace EngConnect.BuildingBlock.Contracts.Abstraction;

/// <summary>
///     Service for handling file storage operations specifically for Google Drive.
/// </summary>
public interface IDriveService
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="file"></param>
    /// <param name="userId"></param>
    /// <param name="fileName"></param>
    /// <param name="prefix"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<FileUploadResult> UploadFileAsync(FileUpload file, Guid userId, string prefix, CancellationToken cancellationToken = default);

    Task<FileUploadResult> UploadMeetingChunkAsync(Guid lessonId, int chunkIndex, FileUpload file,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<FileUploadResult>> GetMeetingChunksAsync(Guid lessonId,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     Retrieves metadata and access information for a specific file from Google Drive.
    /// </summary>
    /// <param name="fileId">The unique identifier of the file in Google Drive.</param>
    /// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
    /// <returns>A result object containing the file details.</returns>
    Task<FileUploadResult> GetFileAsync(string fileId, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Permanently deletes a file from Google Drive.
    /// </summary>
    /// <param name="fileId">The unique identifier of the file to be removed.</param>
    /// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
    /// <returns>A task representing the asynchronous delete operation.</returns>
    Task<bool> DeleteFileAsync(string fileId, CancellationToken cancellationToken = default);
    
    Task<Stream> DownloadFileAsync(string fileId, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="folderName"></param>
    /// <param name="parentId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<string?> FindFolderIdAsync(string folderName, string parentId, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="folderName"></param>
    /// <param name="parentId"></param>ParentFolder
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<string> CreateFolderAsync(string folderName, string parentId, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="folderId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task DeleteFolderAsync(string folderId, CancellationToken cancellationToken = default);

    /// <summary>
    ///
    /// </summary>
    /// <param name="folderName"></param>
    /// <param name="parentFolderId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<string> EnsureFolderExistsAsync(string folderName, string parentFolderId,
        CancellationToken cancellationToken = default);
}