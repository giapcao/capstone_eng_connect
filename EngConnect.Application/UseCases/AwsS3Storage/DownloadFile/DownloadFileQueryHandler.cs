using System.Net;
using System.IO;
using EngConnect.BuildingBlock.Application.Base;
using EngConnect.BuildingBlock.Contracts.Abstraction;
using EngConnect.BuildingBlock.Contracts.Models.Files;
using EngConnect.BuildingBlock.Contracts.Shared;
using EngConnect.BuildingBlock.Domain.DomainErrors;
using Microsoft.Extensions.Logging;

namespace EngConnect.Application.UseCases.AwsS3Storage.DownloadFile;

public class DownloadFileQueryHandler : IQueryHandler<DownloadFileQuery, FileReadResult>
{
    private readonly IAwsStorageService _storageService;
    private readonly ILogger<DownloadFileQueryHandler> _logger;

    public DownloadFileQueryHandler(IAwsStorageService storageService, ILogger<DownloadFileQueryHandler> logger)
    {
        _storageService = storageService;
        _logger = logger;
    }

    public async Task<Result<FileReadResult>> HandleAsync(DownloadFileQuery query, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Start DownloadFileQueryHandler {@query}", query);
        try
        {
            var existFile =await _storageService.FileExistsAsync(query.FileName, cancellationToken);
            if (!existFile)
            { 
                return Result.Failure<FileReadResult>(HttpStatusCode.NotFound, CommonErrors.NotFound<FileReadResult>("File"));
            }
            var file = await _storageService.GetFileStreamAsync(query.FileName, cancellationToken);
            _logger.LogInformation("End DownloadFileQueryHandler");
            return Result.Success(file);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred in DownloadFileQueryHandler: {Message}", ex.Message);
            return Result.Failure<FileReadResult>(HttpStatusCode.InternalServerError, CommonErrors.InternalServerError());
        }
    }
}