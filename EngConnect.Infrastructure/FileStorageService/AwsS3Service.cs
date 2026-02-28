using System.Net;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using EngConnect.BuildingBlock.Contracts.Abstraction;
using EngConnect.BuildingBlock.Contracts.Models.Files;
using EngConnect.BuildingBlock.Contracts.Settings;
using EngConnect.BuildingBlock.Contracts.Shared.Utils;
using Microsoft.Extensions.Options;

namespace EngConnect.Infrastructure.FileStorageService;

public class AwsS3Service : IAwsStorageService
{
    private readonly IAmazonS3 _s3Client;
    private readonly AwsStorageSettings _settings;

    public AwsS3Service(IAmazonS3 s3Client, IOptions<AwsStorageSettings> settings)
    {
        _s3Client =  s3Client;
        _settings = settings.Value;
    }
    public async Task<FileUploadResult> UploadFileAsync(FileUpload fileUpload,Guid userId, 
        string prefix, CancellationToken cancellationToken = default)
    {
        var fileExtension = Path.GetExtension(fileUpload.FileName);
        var storedFileName = string.IsNullOrWhiteSpace(fileUpload.FileName) 
            ? $"{Guid.NewGuid()}{fileExtension}" 
            : fileUpload.FileName;
        
        var dateString = DateTime.UtcNow.ToString("yyMMdd");
        var s3Key = $"{userId}/{prefix}/{dateString}/{storedFileName}";
        using var transferUtility = new TransferUtility(_s3Client);
        var uploadRequest = new TransferUtilityUploadRequest
        {
            InputStream = fileUpload.Content,
            Key = s3Key,
            BucketName = _settings.BucketName,
            ContentType = fileUpload.ContentType
        };

        await transferUtility.UploadAsync(uploadRequest, cancellationToken);
        var url =$"{_settings.CloudFront + s3Key}";
        return new FileUploadResult
        {
            OriginalFileName = fileUpload.FileName,
            StoredFileName = storedFileName,
            Size = fileUpload.Length,
            ContentType = fileUpload.ContentType,
            RelativePath = s3Key,
            RelativePathSystem = null,
            Url = url
        };
    }

    public async Task<Stream> DownloadFileAsync(string fileName, CancellationToken cancellationToken = default)
    {
        var response = await GetS3ObjectAsync(fileName, cancellationToken);
        return response.ResponseStream;
    }

    public async Task<FileReadResult> GetFileStreamAsync(string fileName, CancellationToken cancellationToken = default)
    {
        try
        {
            var request = new GetObjectRequest
            {
                BucketName = _settings.BucketName,
                Key = fileName
            };

            var response = await _s3Client.GetObjectAsync(request, cancellationToken);
            return new FileReadResult
            {
                Stream = response.ResponseStream,
                FileName = Path.GetFileName(fileName),
                ContentType = response.Headers.ContentType,
                Size = response.ContentLength,
                RelativePath = fileName, 
                IsPrivate = true 
            };
        }
        catch (AmazonS3Exception ex)when (ex.StatusCode == HttpStatusCode.NotFound)
        {
            throw new FileNotFoundException($"The file '{fileName}' was not found in storage.");
        }
    }

    public async Task<bool> FileExistsAsync(string fileName, CancellationToken cancellationToken = default)
    {
        try
        {
            var request = new GetObjectMetadataRequest
            {
                BucketName = _settings.BucketName,
                Key = fileName
            };

            await _s3Client.GetObjectMetadataAsync(request, cancellationToken);
            return true;
        }
        catch (AmazonS3Exception ex) when (ex.StatusCode == HttpStatusCode.NotFound)
        {
            return false;
        }
    }

    public string GetPresignedUrl(string storedFileName, int durationMinutes = 15)
    {
        var request = new GetPreSignedUrlRequest
        {
            BucketName = _settings.BucketName,
            Key = storedFileName,
            Verb = HttpVerb.GET,
            Expires = DateTime.UtcNow.AddMinutes(durationMinutes)
        };

        return _s3Client.GetPreSignedURL(request);
    }
    
    public async Task<bool> DeleteFileAsync(string fileName, CancellationToken cancellationToken = default)
    {
        try
        {
            var request = new DeleteObjectRequest
            {
                BucketName = _settings.BucketName,
                Key = fileName
            };

            await _s3Client.DeleteObjectAsync(request, cancellationToken);
            return true;
        }
        catch (AmazonS3Exception)
        {
            return false;
        }
    }
    
    public async Task<FileUploadResult?> UpdateFileAsync(FileUpload fileUpload, Guid userId, string prefix, CancellationToken cancellationToken = default)
    {
        var uploadRequest = await UploadFileAsync(fileUpload, userId, prefix, cancellationToken);
        if (ValidationUtil.IsNotNullOrEmpty(uploadRequest.StoredFileName))
        {
           _ = DeleteFileAsync(fileUpload.FileName, cancellationToken);
            return uploadRequest;
        }

        return null;
    }

    public string GetFileUrl(string? key, CancellationToken cancellationToken = default)
    {
        var url =$"{_settings.CloudFront + key}";
        return url;
    }

    private async Task<GetObjectResponse> GetS3ObjectAsync(string fileName, CancellationToken cancellationToken)
    {
        var request = new GetObjectRequest
        {
            BucketName = _settings.BucketName,
            Key = fileName
        };

        return await _s3Client.GetObjectAsync(request, cancellationToken);
    }
}