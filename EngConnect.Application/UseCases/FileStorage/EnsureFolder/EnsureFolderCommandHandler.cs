using System.Net;
using EngConnect.BuildingBlock.Application.Base;
using EngConnect.BuildingBlock.Contracts.Abstraction;
using EngConnect.BuildingBlock.Contracts.Shared;
using EngConnect.BuildingBlock.Contracts.Shared.Utils;
using EngConnect.BuildingBlock.Domain.DomainErrors;
using Microsoft.Extensions.Logging;

namespace EngConnect.Application.UseCases.FileStorage.EnsureFolder;

public class EnsureFolderCommandHandler : ICommandHandler<EnsureFolderCommand, string>
{
    private readonly IDriveService _driveService;
    private readonly ILogger<EnsureFolderCommandHandler> _logger;

    public EnsureFolderCommandHandler(IDriveService driveService, ILogger<EnsureFolderCommandHandler> logger)
    {
        _driveService = driveService;
        _logger = logger;
    }

    public async Task<Result<string>> HandleAsync(EnsureFolderCommand command, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Start EnsureFolderCommandHandler: {@command}", command);
        try
        {
            if (ValidationUtil.IsNullOrEmpty(command.FolderName))
            {
                _logger.LogWarning("EnsureFolderCommandHandler: FolderName is null or empty");
                return Result.Failure<string>(HttpStatusCode.NotFound, CommonErrors.NotFound<string>("Tên folder"));
            }

            if (ValidationUtil.IsNullOrEmpty(command.ParentFolderId))
            {
                _logger.LogWarning("EnsureFolderCommandHandler: ParentFolderId is null or empty");
                return Result.Failure<string>(HttpStatusCode.NotFound, CommonErrors.NotFound<string>("Parent folder"));
            }

            var folderId = await _driveService.EnsureFolderExistsAsync(command.FolderName, command.ParentFolderId, cancellationToken);

            _logger.LogInformation("End EnsureFolderCommandHandler: FolderId={FolderId}", folderId);
            return Result.Success(folderId);
        }catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred in EnsureFolderCommandHandler: {Message}", ex.Message);
            return Result.Failure<string>(HttpStatusCode.InternalServerError, CommonErrors.InternalServerError());
        }
    }
}
