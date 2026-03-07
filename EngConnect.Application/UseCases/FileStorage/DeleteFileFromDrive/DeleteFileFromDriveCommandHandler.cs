using System.Net;
using EngConnect.BuildingBlock.Application.Base;
using EngConnect.BuildingBlock.Contracts.Abstraction;
using EngConnect.BuildingBlock.Contracts.Shared;
using EngConnect.BuildingBlock.Domain.DomainErrors;
using Microsoft.Extensions.Logging;

namespace EngConnect.Application.UseCases.FileStorage.DeleteFileFromDrive;

public class DeleteFileFromDriveCommandHandler : ICommandHandler<DeleteFileFromDriveCommand>
{
    private readonly IDriveService _driveService;
    private readonly ILogger<DeleteFileFromDriveCommandHandler> _logger;

    public DeleteFileFromDriveCommandHandler(IDriveService driveService, ILogger<DeleteFileFromDriveCommandHandler> logger)
    {
        _driveService = driveService;
        _logger = logger;
    }

    public async Task<Result> HandleAsync(DeleteFileFromDriveCommand command, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Start DeleteFileFromDriveCommandHandler: {@command}", command);
        try
        {
            var result = await _driveService.DeleteFileAsync(command.FileId, cancellationToken);
            if (!result)
            {
                return Result.Failure(HttpStatusCode.BadRequest, CommonErrors.NotFound<string>("FileId"));
            }
            _logger.LogInformation("End DeleteFileFromDriveCommandHandler");
            return Result.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred in DeleteFileFromDriveCommandHandler: {Message}", ex.Message);
            return Result.Failure(HttpStatusCode.InternalServerError, CommonErrors.InternalServerError());
        }
    }
}