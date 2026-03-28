using System.Net;
using EngConnect.Application.UseCases.TutorDocuments.Common;
using EngConnect.BuildingBlock.Application.Base;
using EngConnect.BuildingBlock.Contracts.Abstraction;
using EngConnect.BuildingBlock.Contracts.Shared;
using EngConnect.BuildingBlock.Domain.DomainErrors;
using EngConnect.Domain.Persistence.Models;
using Mapster;
using MapsterMapper;
using Microsoft.Extensions.Logging;

namespace EngConnect.Application.UseCases.TutorDocuments.GetTutorDocumentById;

public class GetTutorDocumentByIdQueryHandler : IQueryHandler<GetTutorDocumentByIdQuery, GetTutorDocumentResponse>
{
    private readonly ILogger<GetTutorDocumentByIdQueryHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IAwsStorageService _awsStorageService;

    public GetTutorDocumentByIdQueryHandler(
        ILogger<GetTutorDocumentByIdQueryHandler> logger,
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IAwsStorageService awsStorageService)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _awsStorageService = awsStorageService;
    }

    public async Task<Result<GetTutorDocumentResponse>> HandleAsync(
        GetTutorDocumentByIdQuery query,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Start GetTutorDocumentByIdQueryHandler {@Query}", query);
        try
        {
            var repo = _unitOfWork.GetRepository<TutorDocument, Guid>();

            var document = await repo.FindSingleAsync(
                x => x.Id == query.Id,
                tracking: false,
                cancellationToken: cancellationToken);

            if (document == null)
            {
                _logger.LogWarning("TutorDocument not found with ID: {Id}", query.Id);
                return Result.Failure<GetTutorDocumentResponse>(
                    HttpStatusCode.NotFound,
                    new Error("TutorDocumentNotFound", "Tài liệu không tồn tại"));
            }

            var result = _mapper.Map<GetTutorDocumentResponse>(document);

            // Convert relative path to full AWS S3 URL
            result.Url = result.Url != null ? _awsStorageService.GetFileUrl(result.Url) : null!;

            _logger.LogInformation("End GetTutorDocumentByIdQueryHandler");
            return Result.Success(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred in GetTutorDocumentByIdQueryHandler: {Message}", ex.Message);
            return Result.Failure<GetTutorDocumentResponse>(
                HttpStatusCode.InternalServerError, CommonErrors.InternalServerError());
        }
    }
}
