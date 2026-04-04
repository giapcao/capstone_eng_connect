using EngConnect.BuildingBlock.Contracts.Models.Files;

namespace EngConnect.BuildingBlock.Contracts.Abstraction;

public interface IAwsStorageService
{
    Task<FileUploadResult> UploadFileAsync(FileUpload fileUpload,Guid userId, 
        string prefix, CancellationToken cancellationToken = default );
    Task<Stream> DownloadFileAsync(string fileName, CancellationToken cancellationToken = default);
    Task<FileReadResult> GetFileStreamAsync(string fileName, CancellationToken cancellationToken = default);
    Task<bool> FileExistsAsync(string fileName, CancellationToken cancellationToken = default);
    string GetPresignedUrl(string storedFileName, int durationMinutes = 15);
    Task<bool> DeleteFileAsync(string fileName, CancellationToken cancellationToken = default);
    Task<FileUploadResult?> UpdateFileAsync(FileUpload fileUpload,Guid userId, 
        string prefix, CancellationToken cancellationToken = default);
    string GetFileUrl(string? key, CancellationToken cancellationToken = default);
    
    Task<FileUploadResult> UploadFileFromPathAsync(string filePath, Guid userId, string prefix, string contentType, CancellationToken cancellationToken = default);
}