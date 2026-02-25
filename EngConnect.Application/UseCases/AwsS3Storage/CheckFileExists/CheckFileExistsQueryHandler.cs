using System.Net;
using EngConnect.BuildingBlock.Application.Base;
using EngConnect.BuildingBlock.Contracts.Abstraction;
using EngConnect.BuildingBlock.Contracts.Shared;
using EngConnect.BuildingBlock.Domain.DomainErrors;
using Microsoft.Extensions.Logging;

namespace EngConnect.Application.UseCases.AwsS3Storage.CheckFileExists;

public class CheckFileExistsQueryHandler : IQueryHandler<CheckFileExistsQuery, bool>
{
    private readonly IAwsStorageService _storageService;
    private readonly ILogger<CheckFileExistsQueryHandler> _logger;

    public CheckFileExistsQueryHandler(IAwsStorageService storageService, ILogger<CheckFileExistsQueryHandler> logger)
    {
        _storageService = storageService;
        _logger = logger;
    }

    public async Task<Result<bool>> HandleAsync(CheckFileExistsQuery query, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Start CheckFileExistsQueryHandler {@query}", query);
        try
        {
            var exists = await _storageService.FileExistsAsync(query.FileName, cancellationToken);
            _logger.LogInformation("End CheckFileExistsQueryHandler");
            return Result.Success(exists);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred in CheckFileExistsQueryHandler: {Message}", ex.Message);
            return Result.Failure<bool>(HttpStatusCode.InternalServerError, CommonErrors.InternalServerError());
        }
    }
}