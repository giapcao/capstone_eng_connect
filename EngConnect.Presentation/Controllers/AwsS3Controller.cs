using EngConnect.Application.UseCases.AwsS3Storage.CheckFileExists;
using EngConnect.Application.UseCases.AwsS3Storage.DeleteFile;
using EngConnect.Application.UseCases.AwsS3Storage.DownloadFile;
using EngConnect.Application.UseCases.AwsS3Storage.GetPresignedUrl;
using EngConnect.Application.UseCases.AwsS3Storage.UploadFile;
using EngConnect.BuildingBlock.Application.Base;
using EngConnect.BuildingBlock.Contracts.Models.Files;
using EngConnect.BuildingBlock.Contracts.Shared;
using EngConnect.BuildingBlock.Presentation.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace EngConnect.Presentation.Controllers;
/// <summary>
/// Quản lý kho lưu trữ AWS S3
/// </summary>
[ApiController]
[Route("api/aws/s3")]
public class AwsS3Controller : BaseApiController
{
    private readonly ICommandDispatcher _commandDispatcher;
    private readonly IQueryDispatcher _queryDispatcher;

    public AwsS3Controller(ICommandDispatcher commandDispatcher, IQueryDispatcher queryDispatcher)
    {
        _commandDispatcher = commandDispatcher;
        _queryDispatcher = queryDispatcher;
    }
    
    /// <summary>
    /// tải file lên 
    /// </summary>
    /// <param name="file"></param>
    /// <param name="userId"></param>
    /// <param name="prefix"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPost("upload")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(Result<FileUploadResult>), StatusCodes.Status200OK)]
    public async Task<IActionResult> UploadFileAsync(IFormFile file, [FromQuery] Guid userId, string prefix, CancellationToken cancellationToken = default)
    {
        var fileUpload = new FileUpload
        {
            FileName = file.FileName,
            ContentType = string.IsNullOrWhiteSpace(file.ContentType) ? "application/octet-stream" : file.ContentType,
            Length = file.Length,
            Content = file.OpenReadStream()
        };

        var command = new UploadFileCommand { File = fileUpload ,  Prefix = prefix , UserId = userId };
        var result = await _commandDispatcher.DispatchAsync(command, cancellationToken);
        return FromResult(result);
    }

    /// <summary>
    /// tải file về máy
    /// </summary>
    /// <param name="fileName"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet("download/{fileName}")]
    [ProducesResponseType(typeof(FileStreamResult), StatusCodes.Status200OK)] 
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DownloadFileAsync([FromRoute] string fileName, CancellationToken cancellationToken = default)
    {
        var query = new DownloadFileQuery { FileName = fileName };
        var result = await _queryDispatcher.DispatchAsync(query, cancellationToken);
        if (result.IsFailure)
        {
            return FromResult(result);
        }
        var fileData = result.Value;
        var contentType = "application/octet-stream";
        return File(fileData!.Stream, contentType, fileData.FileName);
    }

    /// <summary>
    /// Xóa file
    /// </summary>
    /// <param name="fileName"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpDelete("{fileName}")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(Result<bool>), StatusCodes.Status200OK)]
    public async Task<IActionResult> DeleteFileAsync([FromRoute] string fileName, CancellationToken cancellationToken = default)
    {
        var command = new DeleteFileCommand { FileName = fileName };
        var result = await _commandDispatcher.DispatchAsync(command, cancellationToken);
        return FromResult(result);
    }

    /// <summary>
    /// Kiểm tra tồn tại
    /// </summary>
    /// <param name="fileName"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet("exists/{fileName}")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(Result<bool>), StatusCodes.Status200OK)]
    public async Task<IActionResult> CheckFileExistsAsync([FromRoute] string fileName, CancellationToken cancellationToken = default)
    {
        var query = new CheckFileExistsQuery { FileName = fileName };
        var result = await _queryDispatcher.DispatchAsync(query, cancellationToken);
        return FromResult(result);
    }

    /// <summary>
    /// Lấy link ảnh và video
    /// </summary>
    /// <param name="fileName"></param>
    /// <param name="durationMinutes"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet("presigned")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(Result<string>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetPresignedUrlAsync([FromQuery] string fileName, [FromQuery] int durationMinutes = 15, CancellationToken cancellationToken = default)
    {
        var query = new GetPresignedUrlQuery { FileName = fileName, DurationMinutes = durationMinutes };
        var result = await _queryDispatcher.DispatchAsync(query, cancellationToken);
        return FromResult(result);
    }
}