using System.Net;
using EngConnect.BuildingBlock.Application.Base;
using EngConnect.BuildingBlock.Contracts.Abstraction;
using EngConnect.BuildingBlock.Contracts.Shared;
using EngConnect.BuildingBlock.Domain.DomainErrors;
using Microsoft.Extensions.Logging;

namespace EngConnect.Application.UseCases.AwsS3Storage.GetPresignedUrl;

public class GetPresignedUrlQueryHandler : IQueryHandler<GetPresignedUrlQuery, string>
{
    private readonly IAwsStorageService _storageService;
    private readonly ILogger<GetPresignedUrlQueryHandler> _logger;

    public GetPresignedUrlQueryHandler(IAwsStorageService storageService, ILogger<GetPresignedUrlQueryHandler> logger)
    {
        _storageService = storageService;
        _logger = logger;
    }

    public Task<Result<string>> HandleAsync(GetPresignedUrlQuery query, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Start GetPresignedUrlQueryHandler {@query}", query);
        try
        {
            var url = _storageService.GetPresignedUrl(query.FileName, query.DurationMinutes);
            _logger.LogInformation("End GetPresignedUrlQueryHandler");
            return Task.FromResult(Result.Success(url));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred in GetPresignedUrlQueryHandler: {Message}", ex.Message);
            return Task.FromResult(Result.Failure<string>(HttpStatusCode.InternalServerError, CommonErrors.InternalServerError()));
        }
    }
}