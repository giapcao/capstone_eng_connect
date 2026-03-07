using EngConnect.Application.UseCases.FileStorage.CreateFolder;
using EngConnect.Application.UseCases.FileStorage.DeleteFileFromDrive;
using EngConnect.Application.UseCases.FileStorage.DeleteFolder;
using EngConnect.Application.UseCases.FileStorage.EnsureFolder;
using EngConnect.Application.UseCases.FileStorage.GetFileFromDrive;
using EngConnect.Application.UseCases.FileStorage.GetFolder;
using EngConnect.Application.UseCases.FileStorage.UploadFileToDrive;
using EngConnect.BuildingBlock.Application.Base;
using EngConnect.BuildingBlock.Contracts.Models.Files;
using EngConnect.BuildingBlock.Contracts.Shared;
using EngConnect.BuildingBlock.Presentation.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace EngConnect.Presentation.Controllers;

[ApiController]
[Route("api/files")]
public class FileStorageController : BaseApiController
{
    private readonly ICommandDispatcher _commandDispatcher;

    public FileStorageController(ICommandDispatcher commandDispatcher)
    {
        _commandDispatcher = commandDispatcher;
    }

    /// <summary>
    /// Tải file lên google drive
    /// </summary>
    [HttpPost("upload")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(Result<FileUploadResult>), StatusCodes.Status200OK)]
    public async Task<IActionResult> UploadFileAsync(IFormFile file, Guid userId, string fileName, string prefix, CancellationToken cancellationToken = default)
    {
        var fileUpload = new FileUpload
        {
            FileName = fileName,
            ContentType = string.IsNullOrWhiteSpace(file.ContentType) ? "application/octet-stream" : file.ContentType,
            Length = file.Length,
            Content = file.OpenReadStream()
        };

        var command = new UploadFileToDriveCommand { File = fileUpload, UserId = userId, Prefix = prefix };
        var result = await _commandDispatcher.DispatchAsync(command, cancellationToken);
        return FromResult(result);
    }

    /// <summary>
    /// Lấy file và link truy cập
    /// </summary>
    [HttpGet("{fileId}")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(Result<FileUploadResult>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetFileAsync([FromRoute] string fileId,
        CancellationToken cancellationToken = default)
    {
        var command = new GetFileFromDriveCommand { FileId = fileId };
        var result = await _commandDispatcher.DispatchAsync(command, cancellationToken);
        return FromResult(result);
    }

    /// <summary>
    /// Xóa file khỏi drive
    /// </summary>
    [HttpDelete("{fileId}")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(Result), StatusCodes.Status200OK)]
    public async Task<IActionResult> DeleteFileAsync([FromRoute] string fileId,
        CancellationToken cancellationToken = default)
    {
        var command = new DeleteFileFromDriveCommand { FileId = fileId };
        var result = await _commandDispatcher.DispatchAsync(command, cancellationToken);
        return FromResult(result);
    }

    /// <summary>
    /// Tạo folder mới trong google drive
    /// </summary>
    [HttpPost("folders")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(Result<string>), StatusCodes.Status200OK)]
    public async Task<IActionResult> CreateFolderAsync([FromBody] CreateFolderCommand command,
        CancellationToken cancellationToken = default)
    {
        var result = await _commandDispatcher.DispatchAsync(command, cancellationToken);
        return FromResult(result);
    }

    /// <summary>
    /// Tìm folder theo tên
    /// </summary>
    [HttpPost("folders/find")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(Result<string>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetFolderAsync([FromBody] GetFolderCommand command,
        CancellationToken cancellationToken = default)
    {
        var result = await _commandDispatcher.DispatchAsync(command, cancellationToken);
        return FromResult(result);
    }

    /// <summary>
    /// Xóa folder khỏi google drive
    /// </summary>
    [HttpDelete("folders/{folderId}")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(Result), StatusCodes.Status200OK)]
    public async Task<IActionResult> DeleteFolderAsync([FromRoute] string folderId,
        CancellationToken cancellationToken = default)
    {
        var command = new DeleteFolderCommand { FolderId = folderId };
        var result = await _commandDispatcher.DispatchAsync(command, cancellationToken);
        return FromResult(result);
    }

    /// <summary>
    /// Đảm bảo folder tồn tại, tạo mới nếu chưa có
    /// </summary>
    [HttpPost("folders/ensure")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(Result<string>), StatusCodes.Status200OK)]
    public async Task<IActionResult> EnsureFolderAsync([FromBody] EnsureFolderCommand command,
        CancellationToken cancellationToken = default)
    {
        var result = await _commandDispatcher.DispatchAsync(command, cancellationToken);
        return FromResult(result);
    }
}