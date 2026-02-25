using System.Net;
using EngConnect.BuildingBlock.Application.Base;
using EngConnect.BuildingBlock.Contracts.Abstraction;
using EngConnect.BuildingBlock.Contracts.Models.Files;
using EngConnect.BuildingBlock.Contracts.Shared;
using EngConnect.BuildingBlock.Domain.DomainErrors;
using Microsoft.Extensions.Logging;

namespace EngConnect.Application.UseCases.AwsS3Storage.UploadFile;

public class UploadFileCommandHandler : ICommandHandler<UploadFileCommand, FileUploadResult>
{
    private readonly IAwsStorageService _storageService;
    private readonly ILogger<UploadFileCommandHandler> _logger;

    public UploadFileCommandHandler(IAwsStorageService storageService, ILogger<UploadFileCommandHandler> logger)
    {
        _storageService = storageService;
        _logger = logger;
    }

    public async Task<Result<FileUploadResult>> HandleAsync(UploadFileCommand command, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Start UploadFileCommandHandler {@command}", command);
        try
        {
            var result = await _storageService.UploadFileAsync(command.File,command.UserId, 
                command.Prefix, cancellationToken);
            _logger.LogInformation("End UploadFileCommandHandler");
            return Result.Success(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred in UploadFileCommandHandler: {Message}", ex.Message);
            return Result.Failure<FileUploadResult>(HttpStatusCode.InternalServerError, CommonErrors.InternalServerError());
        }
    }
}