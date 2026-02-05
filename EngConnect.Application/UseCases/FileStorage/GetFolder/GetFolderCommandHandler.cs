using System.Net;
using EngConnect.BuildingBlock.Application.Base;
using EngConnect.BuildingBlock.Contracts.Abstraction;
using EngConnect.BuildingBlock.Contracts.Shared;
using EngConnect.BuildingBlock.Contracts.Shared.Utils;
using EngConnect.BuildingBlock.Domain.DomainErrors;
using Microsoft.Extensions.Logging;

namespace EngConnect.Application.UseCases.FileStorage.GetFolder;

public class GetFolderCommandHandler : ICommandHandler<GetFolderCommand, string>
{
    private readonly IDriveService _driveService;
    private readonly ILogger<GetFolderCommandHandler> _logger;

    public GetFolderCommandHandler(IDriveService driveService, ILogger<GetFolderCommandHandler> logger)
    {
        _driveService = driveService;
        _logger = logger;
    }

    public async Task<Result<string>> HandleAsync(GetFolderCommand command, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Start GetFolderCommandHandler: {@command}", command);
        try
        {
            if (ValidationUtil.IsNullOrEmpty(command.FolderName))
            {
                _logger.LogWarning("GetFolderCommandHandler: FolderName is null or empty");
                return Result.Failure<string>(HttpStatusCode.NotFound, CommonErrors.NotFound<string>("Tên Folder"));
            }

            if (ValidationUtil.IsNullOrEmpty(command.ParentFolderId))
            {
                _logger.LogWarning("GetFolderCommandHandler: ParentFolderId is null or empty");
                return Result.Failure<string>(HttpStatusCode.NotFound, CommonErrors.NotFound<string>("Parent folder"));
            }

            var folderId = await _driveService.FindFolderIdAsync(command.FolderName, command.ParentFolderId, cancellationToken);

            if (ValidationUtil.IsNullOrEmpty(folderId))
            {
                _logger.LogWarning("GetFolderCommandHandler: Folder {FolderName} not found", command.FolderName);
                return Result.Failure<string>(HttpStatusCode.NotFound, CommonErrors.NotFound<string>("FolderId"));
            }

            _logger.LogInformation("End GetFolderCommandHandler: FolderId={FolderId}", folderId);
            return Result.Success(folderId);
        }catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred in GetFolderCommandHandler: {Message}", ex.Message);
            return Result.Failure<string>(HttpStatusCode.InternalServerError, CommonErrors.InternalServerError());
        }
    }
}
