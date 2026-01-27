using System.Net;
using EngConnect.Application.UseCases.Lessons.Common;
using EngConnect.BuildingBlock.Application.Base;
using EngConnect.BuildingBlock.Contracts.Abstraction;
using EngConnect.BuildingBlock.Contracts.Shared;
using EngConnect.BuildingBlock.Domain.DomainErrors;
using EngConnect.Domain.Persistence.Models;
using Mapster;
using Microsoft.Extensions.Logging;

namespace EngConnect.Application.UseCases.Lessons.GetLessonById;

public class GetLessonByIdQueryHandler : IQueryHandler<GetLessonByIdQuery, GetLessonResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<GetLessonByIdQueryHandler> _logger;

    public GetLessonByIdQueryHandler(IUnitOfWork unitOfWork, ILogger<GetLessonByIdQueryHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result<GetLessonResponse>> HandleAsync(
        GetLessonByIdQuery query,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Start GetLessonByIdQueryHandler: {@query}", query);
        try
        {
            var lesson = await _unitOfWork.GetRepository<Lesson, Guid>()
                .FindByIdAsync(query.Id, cancellationToken: cancellationToken);

            if (lesson == null)
            {
                _logger.LogWarning("Lesson not found: {id}", query.Id);
                return Result.Failure<GetLessonResponse>(
                    HttpStatusCode.NotFound,
                    CommonErrors.NotFound<Lesson>("Bài học"));
            }

            var lessonResponse = lesson.Adapt<GetLessonResponse>();
            
            _logger.LogInformation("End GetLessonByIdQueryHandler");
            return Result.Success(lessonResponse);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred in GetLessonByIdQueryHandler: {@Message}", ex.Message);
            return Result.Failure<GetLessonResponse>(
                HttpStatusCode.InternalServerError,
                CommonErrors.InternalServerError());
        }
    }
}
