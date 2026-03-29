using System.Linq.Expressions;
using System.Net;
using EngConnect.Application.UseCases.TutorDocuments.Common;
using EngConnect.BuildingBlock.Application.Base;
using EngConnect.BuildingBlock.Application.Utils;
using EngConnect.BuildingBlock.Contracts.Abstraction;
using EngConnect.BuildingBlock.Contracts.Shared;
using EngConnect.BuildingBlock.Contracts.Shared.Utils;
using EngConnect.BuildingBlock.Domain.DomainErrors;
using EngConnect.Domain.Persistence.Models;
using Microsoft.Extensions.Logging;

namespace EngConnect.Application.UseCases.TutorDocuments.GetListTutorDocument;

public class GetListTutorDocumentQueryHandler
    : IQueryHandler<GetListTutorDocumentQuery, PaginationResult<GetTutorDocumentResponse>>
{
    private readonly ILogger<GetListTutorDocumentQueryHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IAwsStorageService _awsStorageService;

    public GetListTutorDocumentQueryHandler(
        ILogger<GetListTutorDocumentQueryHandler> logger,
        IUnitOfWork unitOfWork,
        IAwsStorageService awsStorageService)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _awsStorageService = awsStorageService;
    }

    public async Task<Result<PaginationResult<GetTutorDocumentResponse>>> HandleAsync(
        GetListTutorDocumentQuery query,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Start GetListTutorDocumentQueryHandler {@Query}", query);
        try
        {
            var repo = _unitOfWork.GetRepository<TutorDocument, Guid>().FindAll();

            Expression<Func<TutorDocument, bool>> predicate = x => true;

            if (query.TutorId.HasValue)
            {
                predicate = predicate.CombineAndAlsoExpressions(x => x.TutorId == query.TutorId.Value);
            }

            if (ValidationUtil.IsNotNullOrEmpty(query.DocType))
            {
                predicate = predicate.CombineAndAlsoExpressions(x => x.DocType == query.DocType);
            }

            if (ValidationUtil.IsNotNullOrEmpty(query.Status))
            {
                predicate = predicate.CombineAndAlsoExpressions(x => x.Status == query.Status);
            }

            repo = repo.Where(predicate)
                .ApplySearch(query.GetSearchParams(), x => x.Name ?? string.Empty)
                .ApplySorting(query.GetSortParams());

            var result = await repo.ProjectToPaginatedListAsync<TutorDocument, GetTutorDocumentResponse>(
                query.GetPaginationParams());

            // Convert relative paths to full AWS S3 URLs
            foreach (var item in result.Items)
            {
                item.Url = item.Url != null ? _awsStorageService.GetFileUrl(item.Url) : null!;
            }

            _logger.LogInformation("End GetListTutorDocumentQueryHandler");
            return Result.Success(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred in GetListTutorDocumentQueryHandler: {Message}", ex.Message);
            return Result.Failure<PaginationResult<GetTutorDocumentResponse>>(
                HttpStatusCode.InternalServerError, CommonErrors.InternalServerError());
        }
    }
}
