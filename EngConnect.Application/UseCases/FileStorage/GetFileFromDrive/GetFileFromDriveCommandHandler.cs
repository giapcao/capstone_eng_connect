using System.Net;
using EngConnect.BuildingBlock.Application.Base;
using EngConnect.BuildingBlock.Contracts.Abstraction;
using EngConnect.BuildingBlock.Contracts.Models.Files;
using EngConnect.BuildingBlock.Contracts.Shared;
using EngConnect.BuildingBlock.Domain.DomainErrors;
using Microsoft.Extensions.Logging;

namespace EngConnect.Application.UseCases.FileStorage.GetFileFromDrive;

public class GetFileFromDriveCommandHandler : ICommandHandler<GetFileFromDriveCommand, FileUploadResult>
{
    private readonly IDriveService _driveService;
    private readonly ILogger<GetFileFromDriveCommandHandler> _logger;

    public GetFileFromDriveCommandHandler(IDriveService driveService, ILogger<GetFileFromDriveCommandHandler> logger)
    {
        _driveService = driveService;
        _logger = logger;
    }

    public async Task<Result<FileUploadResult>> HandleAsync(GetFileFromDriveCommand command, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Start GetFileFromDriveCommandHandler: {@command}", command);
        try
        {
            var fileInfo = await _driveService.GetFileAsync(command.FileId, cancellationToken);
            _logger.LogInformation("End GetFileFromDriveCommandHandler");
            return Result.Success(fileInfo);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred in GetFileFromDriveCommandHandler: {Message}", ex.Message);
            return Result.Failure<FileUploadResult>(HttpStatusCode.InternalServerError, CommonErrors.InternalServerError());
        }
    }
}