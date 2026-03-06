using System.Net;
using EngConnect.BuildingBlock.Application.Base;
using EngConnect.BuildingBlock.Contracts.Abstraction;
using EngConnect.BuildingBlock.Contracts.Shared;
using EngConnect.BuildingBlock.Contracts.Shared.Utils;
using EngConnect.BuildingBlock.Domain.DomainErrors;
using Microsoft.Extensions.Logging;

namespace EngConnect.Application.UseCases.FileStorage.DeleteFolder;

public class DeleteFolderCommandHandler : ICommandHandler<DeleteFolderCommand>
{
    private readonly IDriveService _driveService;
    private readonly ILogger<DeleteFolderCommandHandler> _logger;

    public DeleteFolderCommandHandler(IDriveService driveService, ILogger<DeleteFolderCommandHandler> logger)
    {
        _driveService = driveService;
        _logger = logger;
    }

    public async Task<Result> HandleAsync(DeleteFolderCommand command, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Start DeleteFolderCommandHandler: {@command}", command);
        try
        {
            if (ValidationUtil.IsNullOrEmpty(command.FolderId))
            {
                _logger.LogWarning("DeleteFolderCommandHandler: FolderId is null or empty");
                return Result.Failure<string>(HttpStatusCode.NotFound, CommonErrors.NotFound<string>("FolderId"));
            }

            await _driveService.DeleteFolderAsync(command.FolderId, cancellationToken);

            _logger.LogInformation("End DeleteFolderCommandHandler");
            return Result.Success();
        }catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred in DeleteFolderCommandHandler: {Message}", ex.Message);
            return Result.Failure(HttpStatusCode.InternalServerError, CommonErrors.InternalServerError());
        }
    }
}
