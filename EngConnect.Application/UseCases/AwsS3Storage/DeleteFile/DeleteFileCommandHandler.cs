using System.Net;
using EngConnect.BuildingBlock.Application.Base;
using EngConnect.BuildingBlock.Contracts.Abstraction;
using EngConnect.BuildingBlock.Contracts.Shared;
using EngConnect.BuildingBlock.Domain.DomainErrors;
using Microsoft.Extensions.Logging;

namespace EngConnect.Application.UseCases.AwsS3Storage.DeleteFile;

public class DeleteFileCommandHandler : ICommandHandler<DeleteFileCommand, bool>
{
    private readonly IAwsStorageService _storageService;
    private readonly ILogger<DeleteFileCommandHandler> _logger;

    public DeleteFileCommandHandler(IAwsStorageService storageService, ILogger<DeleteFileCommandHandler> logger)
    {
        _storageService = storageService;
        _logger = logger;
    }

    public async Task<Result<bool>> HandleAsync(DeleteFileCommand command, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Start DeleteFileCommandHandler {@command}", command);
        try
        {
            var success = await _storageService.DeleteFileAsync(command.FileName, cancellationToken);
            _logger.LogInformation("End DeleteFileCommandHandler");
            return Result.Success(success);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred in DeleteFileCommandHandler: {Message}", ex.Message);
            return Result.Failure<bool>(HttpStatusCode.InternalServerError, CommonErrors.InternalServerError());
        }
    }
}