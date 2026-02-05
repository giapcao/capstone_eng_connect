using System.Net;
using EngConnect.BuildingBlock.Application.Base;
using EngConnect.BuildingBlock.Contracts.Abstraction;
using EngConnect.BuildingBlock.Contracts.Models.Files;
using EngConnect.BuildingBlock.Contracts.Shared;
using EngConnect.BuildingBlock.Domain.DomainErrors;
using Microsoft.Extensions.Logging;

namespace EngConnect.Application.UseCases.FileStorage.UploadFileToDrive;

public class UploadFileToDriveCommandHandler : ICommandHandler<UploadFileToDriveCommand, FileUploadResult>
{
    private readonly IDriveService _driveService;
    private readonly ILogger<UploadFileToDriveCommandHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public UploadFileToDriveCommandHandler(IDriveService driveService, IUnitOfWork unitOfWork, ILogger<UploadFileToDriveCommandHandler> logger)
    {
        _driveService = driveService;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result<FileUploadResult>> HandleAsync(UploadFileToDriveCommand command, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Start UploadFileToDriveCommandHandler: {@command}", command);
        try
        {
            // var userExist = await _unitOfWork.GetRepository<Domain.Persistence.Models.User,Guid>()
            //     .AnyAsync(x=>x.Id == command.UserId, cancellationToken);
            // if (!userExist)
            // {
            //     return Result.Failure<FileUploadResult>(HttpStatusCode.NotFound,
            //         CommonErrors.NotFound<Domain.Persistence.Models.User>("UserId"));
            // }
            var fileUpload = await _driveService.UploadFileAsync(command.File, command.UserId,command.Prefix, cancellationToken);
            _logger.LogInformation("End UploadFileToDriveCommandHandler");
            return Result.Success(fileUpload);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred in UploadFileToDriveCommandHandler: {Message}", ex.Message);
            return Result.Failure<FileUploadResult>(HttpStatusCode.InternalServerError, CommonErrors.InternalServerError());
        }
    }
}