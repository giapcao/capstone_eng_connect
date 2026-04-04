using EngConnect.BuildingBlock.Contracts.Abstraction;
using EngConnect.BuildingBlock.Contracts.Models.Files;
using EngConnect.BuildingBlock.Contracts.Settings;
using EngConnect.Domain.Constants;
using Google.Apis.Drive.v3;
using Google.Apis.Drive.v3.Data;
using Google.Apis.Upload;
using Microsoft.Extensions.Options;
using File = Google.Apis.Drive.v3.Data.File;

namespace EngConnect.Infrastructure.FileStorageService;

public class GoogleDriveService : IDriveService
{
    private const string MeetingRecordingFolder = "meeting-recordings";
    private const string LessonChunkFolder = "chunks";
    private readonly DriveService _driveService;
    private readonly GoogleDriveSettings _settings;

    public GoogleDriveService(DriveService driveService, IOptions<GoogleDriveSettings>  settings)
    {
        _driveService = driveService;
        _settings = settings.Value;
    }

    public async Task<FileUploadResult> UploadMeetingChunkAsync(Guid lessonId, long chunkTimestamp, FileUpload file,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var chunkFolderId = await EnsureMeetingChunkFolderExistsAsync(lessonId, cancellationToken);
            var extension = Path.GetExtension(file.FileName);
            if (string.IsNullOrWhiteSpace(extension))
            {
                extension = ".webm";
            }

            var chunkFileName = $"chunk-{chunkTimestamp}{extension}";

            var fileMetadata = new File
            {
                Name = chunkFileName,
                Parents = new List<string> { chunkFolderId }
            };

            if (file.Content.CanSeek)
            {
                file.Content.Position = 0;
            }

            var request = _driveService.Files.Create(fileMetadata, file.Content, file.ContentType);
            request.SupportsAllDrives = true;
            request.Fields = "id, name, size, webViewLink, mimeType";

            var uploadProgress = await request.UploadAsync(cancellationToken);
            if (uploadProgress.Status != UploadStatus.Completed || request.ResponseBody == null ||
                string.IsNullOrWhiteSpace(request.ResponseBody.Id))
            {
                throw new Exception($"Failed to upload meeting chunk. Upload status: {uploadProgress.Status}");
            }

            return new FileUploadResult
            {
                OriginalFileName = file.FileName,
                StoredFileName = request.ResponseBody.Name,
                Size = request.ResponseBody.Size ?? file.Length,
                ContentType = request.ResponseBody.MimeType,
                RelativePath = request.ResponseBody.Id,
                RelativePathSystem = $"{DriveConstant.RootFolderName}/{MeetingRecordingFolder}/lesson-{lessonId}/{LessonChunkFolder}/{chunkFileName}",
                Url = $"https://drive.google.com/file/d/{request.ResponseBody.Id}/preview"
            };
        }
        catch (Exception ex)
        {
            throw new Exception($"Failed to upload meeting chunk for lesson '{lessonId}'.", ex);
        }
    }

    public async Task<IReadOnlyList<FileUploadResult>> GetMeetingChunksAsync(Guid lessonId,
        CancellationToken cancellationToken = default)
    {
        var chunkFolderId = await TryGetMeetingChunkFolderIdAsync(lessonId, cancellationToken);
        if (string.IsNullOrWhiteSpace(chunkFolderId))
        {
            return [];
        }

        var listRequest = _driveService.Files.List();
        listRequest.Q = $"'{chunkFolderId}' in parents and trashed = false";
        listRequest.Fields = "files(id, name, size, mimeType, webViewLink)";
        listRequest.SupportsAllDrives = true;
        listRequest.IncludeItemsFromAllDrives = true;

        var files = await listRequest.ExecuteAsync(cancellationToken);
        return files.Files?
            .Select(x => new FileUploadResult
            {
                OriginalFileName = x.Name,
                StoredFileName = x.Name,
                Size = x.Size ?? 0,
                ContentType = x.MimeType,
                RelativePath = x.Id,
                RelativePathSystem = $"{DriveConstant.RootFolderName}/{MeetingRecordingFolder}/lesson-{lessonId}/{LessonChunkFolder}/{x.Name}",
                Url = $"https://drive.google.com/file/d/{x.Id}/preview"
            })
            .ToList() ?? [];
    }
    
    
    public async Task<FileUploadResult> UploadFileAsync(FileUpload file, Guid userId, string prefix, CancellationToken cancellationToken = default)
    {
        try {
            var currentParentId = _settings.MainFolderId;

            if (string.IsNullOrEmpty(currentParentId))
            {
                throw new Exception("MainFolderId (EngConnect root) is not configured.");
            }
                
            var dateFolder = DateTime.UtcNow.ToString("ddMMyy");
            var pathSegments = new[] { userId.ToString(), prefix, dateFolder };
                
            foreach (var folderName in pathSegments)
            {
                if (string.IsNullOrWhiteSpace(folderName)) continue; 
                currentParentId = await EnsureFolderExistsAsync(folderName, currentParentId, cancellationToken);
            }
            
            var fileMetadata = new File()
            {
                Name = file.FileName, 
                Parents = new List<string> { currentParentId }
            };
                
            var request = _driveService.Files.Create(fileMetadata, file.Content, file.ContentType);
            request.SupportsAllDrives = true;
            request.Fields = "id, name, size, webViewLink, mimeType";
            
            var uploadProgress = await request.UploadAsync(cancellationToken);
                
            if (uploadProgress.Status != UploadStatus.Completed)
            {
                throw new Exception($"Upload status: {uploadProgress.Status}. {uploadProgress.Exception?.Message}");
            }
            
            if (request.ResponseBody == null || string.IsNullOrEmpty(request.ResponseBody.Id))
            {
                throw new Exception("Upload completed but no File ID was returned.");
            }
            var fullRelativePath = $"{DriveConstant.RootFolderName}/{userId}/{prefix}/{dateFolder}/{file.FileName}";
            var permission = new Permission() { Type = "anyone", Role = "reader" };
            await _driveService.Permissions.Create(permission, request.ResponseBody.Id).ExecuteAsync(cancellationToken);

            return new FileUploadResult
            {
                OriginalFileName = file.FileName,
                StoredFileName = request.ResponseBody.Name, 
                Size = request.ResponseBody.Size ?? 0,
                ContentType = request.ResponseBody.MimeType,
                RelativePath = request.ResponseBody.Id,
                RelativePathSystem = fullRelativePath,
                Url = $"https://drive.google.com/file/d/{request.ResponseBody.Id}/preview"
            };
        }catch (Exception ex)
        {
                throw new Exception($"Failed to upload file '{file.FileName}' to Drive.", ex);
        }
    }   

    public async Task<FileUploadResult> GetFileAsync(string fileId, CancellationToken cancellationToken = default)
    {
        var request = _driveService.Files.Get(fileId);
        request.Fields = "id, name, size, webViewLink, mimeType";
        var file = await request.ExecuteAsync(cancellationToken);
        return new FileUploadResult
        {
            OriginalFileName = file.Name,
            StoredFileName = file.Name, 
            Size = file.Size ?? 0,
            ContentType = file.MimeType,
            RelativePath = file.Id,
            Url = $"https://drive.google.com/file/d/{file.Id}/preview"
        };
    }

    public async Task<bool> DeleteFileAsync(string fileId, CancellationToken cancellationToken = default)
    {
        try
        {
            await _driveService.Files.Delete(fileId).ExecuteAsync(cancellationToken);
            return true;
        }catch(Exception)
        {
            return false;
        }
    }

    public async Task<Stream> DownloadFileAsync(string fileId, CancellationToken cancellationToken = default)
    {
        var request = _driveService.Files.Get(fileId);

        var stream = new MemoryStream();

        await request.DownloadAsync(stream, cancellationToken);
        
        stream.Position = 0;
        return stream;
    }

    public async Task<string?> FindFolderIdAsync(string folderName, string parentId, CancellationToken cancellationToken = default)
{
    try
    {
        var listRequest = _driveService.Files.List();
        listRequest.Q = $"mimeType = 'application/vnd.google-apps.folder' and name = '{folderName}' and '{parentId}' in parents and trashed = false";
        listRequest.Fields = "files(id)";
        listRequest.PageSize = 1; 

        var result = await listRequest.ExecuteAsync(cancellationToken);

        return result.Files?.FirstOrDefault()?.Id;
    }
    catch (Exception ex)
    {
        throw new Exception($"Error finding folder '{folderName}'", ex);
    }
}

    public async Task<string> CreateFolderAsync(string folderName, string parentId, CancellationToken cancellationToken = default)
    {
        try
        {
            var fileMetadata = new File()
            {
            Name = folderName,
            MimeType = "application/vnd.google-apps.folder", 
            Parents = new List<string> { parentId }
            };

            var request = _driveService.Files.Create(fileMetadata);
            request.Fields = "id"; 
            request.SupportsAllDrives = true; 

            var file = await request.ExecuteAsync(cancellationToken);
            
            if (file == null || string.IsNullOrEmpty(file.Id))
            {
                 throw new Exception("Folder created but no ID returned.");
            }

            return file.Id;
        }
        catch (Exception ex)
        {
            throw new Exception($"Failed to create folder '{folderName}'", ex);
        }
    }

    public async Task DeleteFolderAsync(string folderId, CancellationToken cancellationToken = default)
    {
        try
        {
            await _driveService.Files.Delete(folderId).ExecuteAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            throw new Exception($"Failed to delete folder with ID '{folderId}'", ex);
        }
    }
    
    public async Task<string> EnsureFolderExistsAsync(string folderName, string parentFolderId,
        CancellationToken cancellationToken = default)
    {
        var listRequest = _driveService.Files.List();

        listRequest.Q = $"mimeType = 'application/vnd.google-apps.folder' and name = '{folderName}' and '{parentFolderId}' in parents and trashed = false";
        listRequest.Fields = "files(id)";
        listRequest.SupportsAllDrives = true;
        listRequest.IncludeItemsFromAllDrives = true;

        var existingFolders = await listRequest.ExecuteAsync(cancellationToken);

        if (existingFolders.Files is { Count: > 0 })
        {
            return existingFolders.Files[0].Id;
        }

        var folderMetadata = new File()
        {
            Name = folderName,
            MimeType = "application/vnd.google-apps.folder",
            Parents = new List<string> { parentFolderId }
        };

        var createRequest = _driveService.Files.Create(folderMetadata);
        createRequest.Fields = "id";
        createRequest.SupportsAllDrives = true;
        
        var folder = await createRequest.ExecuteAsync(cancellationToken);
        return folder.Id;
    }

    private async Task<string> EnsureMeetingChunkFolderExistsAsync(Guid lessonId,
        CancellationToken cancellationToken = default)
    {
        var currentParentId = _settings.MainFolderId;
        if (string.IsNullOrWhiteSpace(currentParentId))
        {
            throw new Exception("MainFolderId (EngConnect root) is not configured.");
        }

        var pathSegments = new[] { MeetingRecordingFolder, $"lesson-{lessonId}", LessonChunkFolder };
        foreach (var folderName in pathSegments)
        {
            currentParentId = await EnsureFolderExistsAsync(folderName, currentParentId, cancellationToken);
        }

        return currentParentId;
    }

    private async Task<string?> TryGetMeetingChunkFolderIdAsync(Guid lessonId,
        CancellationToken cancellationToken = default)
    {
        var currentParentId = _settings.MainFolderId;
        if (string.IsNullOrWhiteSpace(currentParentId))
        {
            return null;
        }

        var pathSegments = new[] { MeetingRecordingFolder, $"lesson-{lessonId}", LessonChunkFolder };
        foreach (var folderName in pathSegments)
        {
            var folderId = await FindFolderIdAsync(folderName, currentParentId, cancellationToken);
            if (string.IsNullOrWhiteSpace(folderId))
            {
                return null;
            }

            currentParentId = folderId;
        }

        return currentParentId;
    }
}