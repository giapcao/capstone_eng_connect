using System.Net;
using EngConnect.BuildingBlock.Application.Base;
using EngConnect.BuildingBlock.Contracts.Abstraction;
using EngConnect.BuildingBlock.Contracts.Shared;
using EngConnect.BuildingBlock.Contracts.Shared.Utils;
using EngConnect.BuildingBlock.Domain.DomainErrors;
using Microsoft.Extensions.Logging;

namespace EngConnect.Application.UseCases.FileStorage.CreateFolder;

public class CreateFolderCommandHandler : ICommandHandler<CreateFolderCommand, string>
{
    private readonly IDriveService _driveService;
    private readonly ILogger<CreateFolderCommandHandler> _logger;

    public CreateFolderCommandHandler(IDriveService driveService, ILogger<CreateFolderCommandHandler> logger)
    {
        _driveService = driveService;
        _logger = logger;
    }

    public async Task<Result<string>> HandleAsync(CreateFolderCommand command, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Start CreateFolderCommandHandler: {@command}", command);
        try
        {
            if (ValidationUtil.IsNullOrEmpty(command.FolderName))
            {
                _logger.LogWarning("CreateFolderCommandHandler: FolderName is null or empty");
                return Result.Failure<string>(HttpStatusCode.NotFound, CommonErrors.NotFound<string>("Tên folder"));
            }

            if (ValidationUtil.IsNullOrEmpty(command.ParentFolderId))
            {
                _logger.LogWarning("CreateFolderCommandHandler: ParentFolderId is null or empty");
                return Result.Failure<string>(HttpStatusCode.NotFound, CommonErrors.NotFound<string>("Parent folder"));
            }

            var folderId = await _driveService.CreateFolderAsync(command.FolderName, command.ParentFolderId, cancellationToken);

            _logger.LogInformation("End CreateFolderCommandHandler: FolderId={FolderId}", folderId);
            return Result.Success(folderId);
        }catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred in CreateFolderCommandHandler: {Message}", ex.Message);
            return Result.Failure<string>(HttpStatusCode.InternalServerError, CommonErrors.InternalServerError());
        }
    }
}
