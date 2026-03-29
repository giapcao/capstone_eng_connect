using System.Linq.Expressions;
using System.Net;
using EngConnect.Application.UseCases.Students.Common;
using EngConnect.BuildingBlock.Application.Base;
using EngConnect.BuildingBlock.Application.Utils;
using EngConnect.BuildingBlock.Contracts.Abstraction;
using EngConnect.BuildingBlock.Contracts.Shared;
using EngConnect.BuildingBlock.Contracts.Shared.Utils;
using EngConnect.BuildingBlock.Domain.DomainErrors;
using Microsoft.Extensions.Logging;
using EngConnect.Domain.Persistence.Models;

namespace EngConnect.Application.UseCases.Students.GetListStudents;

public class GetListStudentQueryHandler : IQueryHandler<GetListStudentQuery, PaginationResult<GetStudentResponse>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<GetListStudentQueryHandler> _logger;
    private readonly IAwsStorageService _awsStorageService;

    public GetListStudentQueryHandler(ILogger<GetListStudentQueryHandler> logger, IUnitOfWork unitOfWork, IAwsStorageService awsStorageService)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _awsStorageService = awsStorageService;
    }
    
    public async Task<Result<PaginationResult<GetStudentResponse>>> HandleAsync(GetListStudentQuery query, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Start GetListStudentQueryHandler : {@query}",query);
        try
        {
            var students = _unitOfWork.GetRepository<Student, Guid>().FindAll();
            Expression<Func<Student, bool>> predicate = x => true;
            
            if (ValidationUtil.IsNotNullOrEmpty(query.Status))
            {
                predicate = predicate.CombineAndAlsoExpressions(x => x.Status != null && query.Status.ToLower().Contains(x.Status.ToLower()));
            }

            students = students.Where(predicate);

            students = students.ApplySearch(query.GetSearchParams(),
                    x => x.Class ?? string.Empty,
                    x => x.Grade ?? string.Empty,
                    x => x.Notes ?? string.Empty,
                    x => x.School ?? string.Empty)
                .ApplySorting(query.GetSortParams());

            var result =
                await students.ProjectToPaginatedListAsync<Student, GetStudentResponse>
                    (query.GetPaginationParams());

            // Convert relative paths to full AWS S3 URLs
            foreach (var item in result.Items)
            {
                item.Avatar = item.Avatar != null ? _awsStorageService.GetFileUrl(item.Avatar) : null;
            }

            _logger.LogInformation("End GetListStudentQueryHandler)");
            return Result.Success(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred in GetListStudentQueryHandler {@Message}", ex.Message);
            return Result.Failure<PaginationResult<GetStudentResponse>>(HttpStatusCode.InternalServerError,
                CommonErrors.InternalServerError());
        }
    }
}