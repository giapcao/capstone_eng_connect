using System.Linq.Expressions;
using System.Net;
using EngConnect.Application.UseCases.Lessons.Common;
using EngConnect.BuildingBlock.Application.Base;
using EngConnect.BuildingBlock.Application.Utils;
using EngConnect.BuildingBlock.Contracts.Abstraction;
using EngConnect.BuildingBlock.Contracts.Shared;
using EngConnect.BuildingBlock.Contracts.Shared.Utils;
using EngConnect.BuildingBlock.Domain.DomainErrors;
using EngConnect.Domain.Persistence.Models;
using Microsoft.Extensions.Logging;

namespace EngConnect.Application.UseCases.Lessons.GetListLessons;

public class GetListLessonQueryHandler : IQueryHandler<GetListLessonQuery, PaginationResult<GetLessonResponse>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<GetListLessonQueryHandler> _logger;

    public GetListLessonQueryHandler(IUnitOfWork unitOfWork, ILogger<GetListLessonQueryHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result<PaginationResult<GetLessonResponse>>> HandleAsync(
        GetListLessonQuery query,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Start GetListLessonQueryHandler: {@query}", query);
        try
        {
            var lessonRepository = _unitOfWork.GetRepository<Lesson, Guid>();
            
            var lessons = lessonRepository.FindAll();
            
            Expression<Func<Lesson, bool>> predicate = x => true;
            
            if (ValidationUtil.IsNotNullOrEmpty(query.Status))
            {
                predicate = predicate.CombineAndAlsoExpressions(x => x.Status != null && query.Status.ToLower().Contains(x.Status.ToLower()));
            }
            
            if (query.StartTimeFrom.HasValue)
                predicate = predicate.CombineAndAlsoExpressions(x => x.StartTime >= query.StartTimeFrom);
            
            if (query.StartTimeTo.HasValue)
                predicate = predicate.CombineAndAlsoExpressions(x => x.StartTime <= query.StartTimeTo);

            if (query.StudentId.HasValue)
            {
                predicate = predicate.CombineAndAlsoExpressions(x => x.StudentId == query.StudentId);
            }
            
            lessons = lessons.Where(predicate);
            
            lessons = lessons.ApplySorting(query.GetSortParams());

            var result =
                await lessons.ProjectToPaginatedListAsync<Lesson, GetLessonResponse>
                    (query.GetPaginationParams());
            
            _logger.LogInformation("End GetListLessonQueryHandler");
            return Result.Success(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred in GetListLessonQueryHandler: {@Message}", ex.Message);
            return Result.Failure<PaginationResult<GetLessonResponse>>(
                HttpStatusCode.InternalServerError,
                CommonErrors.InternalServerError());
        }
    }
}
