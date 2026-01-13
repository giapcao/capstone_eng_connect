using System.Net;
using System.Security;
using System.Text.RegularExpressions;
using EngConnect.BuildingBlock.Contracts.Abstraction;
using EngConnect.BuildingBlock.Contracts.Models.Files;
using EngConnect.BuildingBlock.Contracts.Settings;
using EngConnect.BuildingBlock.Contracts.Shared;
using EngConnect.BuildingBlock.Contracts.Shared.Utils;
using EngConnect.BuildingBlock.Domain.DomainErrors;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace EngConnect.BuildingBlock.Infrastructure.FileStorage;

public class FileStorageService : IFileStorageService
{
    private readonly FileStorageSettings _settings;
    private readonly string _publicRoot;
    private readonly string _privateRoot;
    private readonly ILogger<FileStorageService> _logger;

    public FileStorageService(IOptions<FileStorageSettings> settings, ILogger<FileStorageService> logger)
    {
        _settings = settings.Value;
        _publicRoot = ResolveBasePath(_settings.PublicFileBasePath);
        _privateRoot = ResolveBasePath(_settings.PrivateFileBasePath);
        _logger = logger;

    }

    public async Task<Result<FileUploadResult>> UploadFileAsync(FileUpload file, bool isPrivate, string? directory = null,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Start UploadFileAsync for file {FileName}, isPrivate: {IsPrivate}, directory: {Directory}", file.FileName, isPrivate, directory);
        try
        {
            var validationResult = ValidateFile(file);
            if (validationResult.IsFailure)
            {
                _logger.LogWarning("UploadFileAsync failed: {@Error}", validationResult.Error);
                return Result.Failure<FileUploadResult>(HttpStatusCode.BadRequest, validationResult.Error!);
            }

            var basePath = GetBasePath(isPrivate);
            var targetDir = BuildFullPath(basePath, directory ?? string.Empty);

            if (!Directory.Exists(targetDir))
            {
                Directory.CreateDirectory(targetDir);
            }

            var fileName = _settings.UseUniqueFilenames ? $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}" : SanitizeFileName(file.FileName);

            var filePath = Path.Combine(targetDir, fileName);
            await using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                file.Content.Position = 0;
                await file.Content.CopyToAsync(fileStream, cancellationToken);
            }

            var result = BuildUploadResult(filePath, file.FileName, file.ContentType, basePath, isPrivate);

            _logger.LogInformation("End UploadFileAsync");
            return Result.Success(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred in UploadFileAsync: {Message}", ex.Message);
            return Result.Failure<FileUploadResult>(HttpStatusCode.InternalServerError, CommonErrors.InternalServerError());
        }
    }

    public Result<FileReadResult> GetFile(string relativePath, bool isPrivate)
    {
        _logger.LogInformation("Start GetFile for path {RelativePath}, isPrivate: {IsPrivate}", relativePath, isPrivate);
        try
        {
            if (string.IsNullOrWhiteSpace(relativePath))
            {
                _logger.LogWarning("GetFile failed: Invalid file path");
                return Result.Failure<FileReadResult>(HttpStatusCode.BadRequest, FileErrors.InvalidPath("File path is required"));
            }

            var fullPath = BuildFullPath(GetBasePath(isPrivate), relativePath);

            if (!File.Exists(fullPath))
            {
                _logger.LogWarning("GetFile failed: File not found at path {FullPath}", fullPath);
                return Result.Failure<FileReadResult>(HttpStatusCode.BadRequest, FileErrors.NotFound(relativePath));
            }

            var stream = new FileStream(fullPath, FileMode.Open, FileAccess.Read, FileShare.Read);
            var response = new FileReadResult
            {
                FileName = Path.GetFileName(fullPath),
                ContentType = ResolveContentType(fullPath),
                Stream = stream,
                Size = new FileInfo(fullPath).Length,
                RelativePath = GetRelativePath(fullPath, GetBasePath(isPrivate)),
                IsPrivate = isPrivate
            };

            _logger.LogInformation("End GetFile");
            return Result.Success(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred in GetFile: {Message}", ex.Message);
            return Result.Failure<FileReadResult>(HttpStatusCode.InternalServerError, CommonErrors.InternalServerError());
        }
    }

    public Result DeleteFile(string relativePath, bool isPrivate)
    {
        _logger.LogInformation("Start DeleteFile for path {RelativePath}, isPrivate: {IsPrivate}", relativePath, isPrivate);
        try
        {
            if (string.IsNullOrWhiteSpace(relativePath))
            {
                _logger.LogWarning("DeleteFile failed: Invalid file path");
                return Result.Failure(HttpStatusCode.BadRequest, FileErrors.InvalidPath("File path is required"));
            }

            var fullPath = BuildFullPath(GetBasePath(isPrivate), relativePath);
            if (!File.Exists(fullPath))
            {
                _logger.LogWarning("DeleteFile failed: File not found at path {FullPath}", fullPath);
                return Result.Failure(HttpStatusCode.BadRequest, FileErrors.NotFound(relativePath));
            }

            File.Delete(fullPath);

            _logger.LogInformation("End DeleteFile");
            return Result.Success();
        }
        // Catch file not found exception separately
        catch (FileNotFoundException ex)
        {
            _logger.LogWarning(ex, "DeleteFile failed: File not found exception for path {RelativePath}", relativePath);
            return Result.Failure(HttpStatusCode.BadRequest, FileErrors.NotFound(relativePath));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred in DeleteFile: {Message}", ex.Message);
            return Result.Failure(HttpStatusCode.InternalServerError, CommonErrors.InternalServerError());
        }
    }

    public Result<List<FileEntryResult>> ListFiles(bool isPrivate, string? directory = null,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Start ListFiles for isPrivate: {IsPrivate}, directory: {Directory}", isPrivate, directory);
        try
        {
            var basePath = GetBasePath(isPrivate);
            var scopedPath = string.IsNullOrWhiteSpace(directory) ? basePath : BuildFullPath(basePath, directory);

            if (!Directory.Exists(scopedPath))
            {
                _logger.LogInformation("Directory does not exist at path {ScopedPath}, returning empty list", scopedPath);
                return Result.Success(new List<FileEntryResult>());
            }

            var entries = new List<FileEntryResult>();
            // Include nested folders to return every file under the scoped path
            foreach (var file in Directory.EnumerateFiles(scopedPath, "*", SearchOption.AllDirectories))
            {
                cancellationToken.ThrowIfCancellationRequested();
                var info = new FileInfo(file);
                entries.Add(new FileEntryResult
                {
                    RelativePath = GetRelativePath(file, basePath),
                    FileName = info.Name,
                    Size = info.Length,
                    IsPrivate = isPrivate,
                    LastModifiedUtc = info.LastWriteTimeUtc
                });
            }

            _logger.LogInformation("End ListFiles");
            return Result.Success(entries);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred in ListFiles: {Message}", ex.Message);
            return Result.Failure<List<FileEntryResult>>(HttpStatusCode.InternalServerError, CommonErrors.InternalServerError());
        }
    }

    public Result<FileUploadResult> MoveFile(string relativePath, bool sourceIsPrivate)
    {
        _logger.LogInformation("Start MoveFile for path {RelativePath}, sourceIsPrivate: {SourceIsPrivate}", relativePath, sourceIsPrivate);
        try
        {
            if (ValidationUtil.IsNullOrEmpty(relativePath))
            {
                _logger.LogWarning("MoveFile failed: Invalid file path");
                return Result.Failure<FileUploadResult>(HttpStatusCode.BadRequest, FileErrors.InvalidPath("File path is required"));
            }

            var sanitized = SanitizeRelativePath(relativePath);
            var sourceBase = GetBasePath(sourceIsPrivate);
            var sourcePath = BuildFullPath(sourceBase, sanitized);
            if (!File.Exists(sourcePath))
            {
                _logger.LogWarning("MoveFile failed: File not found in expected {Scope} path at {SourcePath}", sourceIsPrivate ? "private" : "public", sourcePath);
                return Result.Failure<FileUploadResult>(HttpStatusCode.BadRequest, FileErrors.NotFound(relativePath));
            }

            var destinationIsPrivate = !sourceIsPrivate;
            var destinationBase = GetBasePath(destinationIsPrivate);
            var destinationPath = BuildFullPath(destinationBase, sanitized);

            var destinationDir = Path.GetDirectoryName(destinationPath);
            if (!string.IsNullOrWhiteSpace(destinationDir) && !Directory.Exists(destinationDir))
            {
                Directory.CreateDirectory(destinationDir);
            }

            // Overwrite if the destination exists
            if (File.Exists(destinationPath))
            {
                File.Delete(destinationPath);
            }

            File.Move(sourcePath, destinationPath);

            _logger.LogInformation("Moved file from {Source} ({SourceScope}) to {Destination} ({DestinationScope})", 
                sourcePath, sourceIsPrivate ? "private" : "public", 
                destinationPath, destinationIsPrivate ? "private" : "public");
            var result = BuildUploadResult(destinationPath, Path.GetFileName(destinationPath), ResolveContentType(destinationPath), destinationBase, destinationIsPrivate);
            
            _logger.LogInformation("End MoveFile");
            return Result.Success(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred in MoveFile: {Message}", ex.Message);
            return Result.Failure<FileUploadResult>(HttpStatusCode.InternalServerError, CommonErrors.InternalServerError());
        }
    }

    public Result ValidateFile(FileUpload file)
    {
        _logger.LogInformation("Validating file {FileName} of size {FileSize} bytes", file.FileName, file.Length);
        if (file.Length > _settings.MaxFileSize)
        {
            _logger.LogWarning("File validation failed: Exceeded max size of {MaxFileSize} bytes", _settings.MaxFileSize);
            return Result.Failure(HttpStatusCode.BadRequest, FileErrors.ExceededMaxSize(_settings.MaxFileSize));
        }

        var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
        if (!_settings.AllowedExtensions.Contains(extension))
        {
            _logger.LogWarning("File validation failed: Unsupported file type {Extension}", extension);
            return Result.Failure(HttpStatusCode.BadRequest, FileErrors.UnsupportedFileType(extension));
        }

        _logger.LogInformation("File {FileName} passed validation", file.FileName);
        return Result.Success();
    }

    private string GetBasePath(bool isPrivate)
    {
        return isPrivate ? _privateRoot : _publicRoot;
    }

    private static string ResolveBasePath(string basePath)
    {
        var sanitizedBase = basePath ?? string.Empty;
        var combined = Path.GetFullPath(sanitizedBase);

        if (!Directory.Exists(combined))
        {
            Directory.CreateDirectory(combined);
        }

        return combined;
    }

    private string BuildFullPath(string basePath, string relativePath)
    {
        var sanitized = SanitizeRelativePath(relativePath);
        var full = Path.GetFullPath(Path.Combine(basePath, sanitized));

        if (!full.StartsWith(basePath, StringComparison.OrdinalIgnoreCase))
        {
            _logger.LogWarning("Path traversal attempt detected for basePath: {BasePath}, relativePath: {RelativePath}", basePath, relativePath);
            throw new SecurityException("Path traversal attempt detected");
        }

        return full;
    }

    private string SanitizeRelativePath(string path)
    {
        if (string.IsNullOrWhiteSpace(path))
        {
            return string.Empty;
        }

        var normalized = path.Replace('/', Path.DirectorySeparatorChar).Replace('\\', Path.DirectorySeparatorChar);
        normalized = normalized.TrimStart(Path.DirectorySeparatorChar);

        if (normalized.Contains(".." + Path.DirectorySeparatorChar) || normalized.Equals(".."))
        {
            _logger.LogWarning("Path traversal attempt detected for path: {Path}", path);
            throw new SecurityException("Path traversal attempt detected");
        }

        return normalized;
    }

    private static string SanitizeFileName(string fileName)
    {
        var invalidChars = new string(Path.GetInvalidFileNameChars());
        var invalidReStr = $"[{Regex.Escape(invalidChars)}]";
        var sanitized = Regex.Replace(fileName, invalidReStr, string.Empty);

        if (sanitized.Length <= 100) return sanitized;
        var extension = Path.GetExtension(sanitized);
        sanitized = string.Concat(sanitized.AsSpan(0, 100 - extension.Length), extension);

        return sanitized;
    }

    private static string ResolveContentType(string fileName)
    {
        var provider = new Microsoft.AspNetCore.StaticFiles.FileExtensionContentTypeProvider();
        if (provider.TryGetContentType(fileName, out var contentType))
        {
            return contentType;
        }
        return "application/octet-stream";
    }

    private static string GetRelativePath(string fullPath, string basePath)
    {
        var relative = Path.GetRelativePath(basePath, fullPath);
        return relative.Replace('\\', '/');
    }

    private FileUploadResult BuildUploadResult(string fullPath, string originalFileName, string contentType, string basePath, bool isPrivate)
    {
        var relativePath = GetRelativePath(fullPath, basePath);
        var url = isPrivate ? string.Empty : $"{_settings.BaseUrl.TrimEnd('/')}/{relativePath}".Replace("//", "/");

        return new FileUploadResult
        {
            OriginalFileName = originalFileName,
            StoredFileName = Path.GetFileName(fullPath),
            Size = new FileInfo(fullPath).Length,
            ContentType = contentType,
            RelativePath = relativePath,
            Url = url
        };
    }

}