using System.Net;
using EngConnect.BuildingBlock.Application.Base;
using EngConnect.BuildingBlock.Contracts.Abstraction;
using EngConnect.BuildingBlock.Contracts.Models.Files;
using EngConnect.BuildingBlock.Contracts.Shared;
using EngConnect.BuildingBlock.Domain.DomainErrors;
using Microsoft.Extensions.Logging;

namespace EngConnect.Application.UseCases.AwsS3Storage.UploadFileFromPath;

public class UploadFileFromPathCommandHandler : ICommandHandler<UploadFileFromPathCommand, FileUploadResult>
{
    private readonly IAwsStorageService _storageService;
    private readonly ILogger<UploadFileFromPathCommandHandler> _logger;

    public UploadFileFromPathCommandHandler(IAwsStorageService storageService, ILogger<UploadFileFromPathCommandHandler> logger)
    {
        _storageService = storageService;
        _logger = logger;
    }

    public async Task<Result<FileUploadResult>> HandleAsync(UploadFileFromPathCommand command, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Start UploadFileFromPathCommandHandler {@command}", command);
        try
        {
            var result = await _storageService.UploadFileFromPathAsync(command.FilePath, command.UserId,
                command.Prefix, command.ContentType, cancellationToken);
            _logger.LogInformation("End UploadFileFromPathCommandHandler");
            return Result.Success(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred in UploadFileFromPathCommandHandler: {Message}", ex.Message);
            return Result.Failure<FileUploadResult>(HttpStatusCode.InternalServerError, CommonErrors.InternalServerError());
        }
    }
}