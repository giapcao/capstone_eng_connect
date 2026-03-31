using System.Net;
using EngConnect.Application.UseCases.CourseResources.Common;
using EngConnect.BuildingBlock.Application.Base;
using EngConnect.BuildingBlock.Contracts.Abstraction;
using EngConnect.BuildingBlock.Contracts.Shared;
using EngConnect.BuildingBlock.Contracts.Shared.Utils;
using EngConnect.BuildingBlock.Domain.DomainErrors;
using EngConnect.Domain.Constants;
using EngConnect.Domain.DomainErrors;
using EngConnect.Domain.Persistence.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EngConnect.Application.UseCases.CourseResources.CreateCourseResource;

public class CreateCourseResourceCommandHandler : ICommandHandler<CreateCourseResourceCommand, GetCourseResourceResponse>
{
    private readonly ILogger<CreateCourseResourceCommandHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IAwsStorageService _awsStorageService;

    public CreateCourseResourceCommandHandler(ILogger<CreateCourseResourceCommandHandler> logger, IUnitOfWork unitOfWork, IAwsStorageService awsStorageService)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _awsStorageService = awsStorageService;
    }

    public async Task<Result<GetCourseResourceResponse>> HandleAsync(CreateCourseResourceCommand command,
        CancellationToken cancellationToken = default)
    {
        Guid? transactionId = null;
        _logger.LogInformation("Start CreateCourseResourceCommandHandler {@Command}", command);
        try
        {
            var courseResourceRepo = _unitOfWork.GetRepository<CourseResource, Guid>();
            var courseSessionRepo = _unitOfWork.GetRepository<CourseSession, Guid>();
            var courseSessionCourseResourceRepo = _unitOfWork.GetRepository<CourseSessionCourseResource, Guid>();

            var tutorExists = await _unitOfWork.GetRepository<Tutor, Guid>().FindFirstAsync(
                x => x.Id == command.TutorId,
                cancellationToken: cancellationToken);
            if (ValidationUtil.IsNullOrEmpty(tutorExists))
            {
                _logger.LogWarning("Tutor not found with ID: {TutorId}", command.TutorId);
                return Result.Failure<GetCourseResourceResponse>(HttpStatusCode.BadRequest, TutorErrors.TutorNotFound());
            }

            var courseSession = await courseSessionRepo.FindAll(x => x.Id == command.CourseSessionId)
                .Include(x => x.CourseModuleCourseSessions)
                    .ThenInclude(x => x.CourseModule)
                        .ThenInclude(x => x.CourseCourseModules)
                            .ThenInclude(x => x.Course)
                .FirstOrDefaultAsync(cancellationToken);
            if (courseSession == null)
            {
                _logger.LogWarning("Course session not found with ID: {CourseSessionId}", command.CourseSessionId);
                return Result.Failure<GetCourseResourceResponse>(HttpStatusCode.BadRequest,
                    new Error("CourseSessionNotFound", "Session khong ton tai"));
            }

            var hasPublishedCourse = courseSession.CourseModuleCourseSessions
                .SelectMany(x => x.CourseModule.CourseCourseModules)
                .Any(x => x.Course.Status == nameof(CourseStatus.Published));
            if (hasPublishedCourse)
            {
                _logger.LogWarning("Course resources cannot be changed because course session {CourseSessionId} belongs to a published course", command.CourseSessionId);
                return Result.Failure<GetCourseResourceResponse>(HttpStatusCode.BadRequest,
                    CourseErrors.PublishedCourseCannotBeUpdated());
            }

            var transaction = await _unitOfWork.BeginTransactionAsync();
            transactionId = transaction.TransactionId;

            var courseResourceUrl = await _awsStorageService.UploadFileAsync(command.ResourceFile, tutorExists.Id,
                nameof(PrefixFile.CourseResource), cancellationToken);

            var courseResource = new CourseResource
            {
                Id = Guid.NewGuid(),
                TutorId = tutorExists.Id,
                Title = command.Title,
                Url = courseResourceUrl.RelativePath,
                ResourceType = command.ResourceType,
                CreatedAt = DateTime.UtcNow
            };

            courseResourceRepo.Add(courseResource);

            courseSessionCourseResourceRepo.Add(new CourseSessionCourseResource
            {
                Id = Guid.NewGuid(),
                CourseSessionId = command.CourseSessionId,
                CourseResourceId = courseResource.Id
            });

            await _unitOfWork.SaveChangesAsync();

            if (ValidationUtil.IsNotNullOrEmpty(transactionId))
            {
                _logger.LogDebug("Committing transaction with {TransactionId}", transactionId);
                await _unitOfWork.CommitTransactionAsync();
            }

            _logger.LogInformation("End CreateCourseResourceCommandHandler");
            return Result.Success(new GetCourseResourceResponse
            {
                Id = courseResource.Id,
                TutorId = courseResource.TutorId ?? Guid.Empty,
                Title = courseResource.Title,
                ResourceType = courseResource.ResourceType,
                Url = _awsStorageService.GetFileUrl(courseResource.Url),
                Status = courseResource.Status,
                CreatedAt = courseResource.CreatedAt,
                UpdatedAt = courseResource.UpdatedAt
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred in CreateCourseResourceCommandHandler: {Message}", ex.Message);
            if (ValidationUtil.IsNotNullOrEmpty(transactionId))
            {
                _logger.LogDebug("Rolling back transaction with {TransactionId} due to error", transactionId);
                await _unitOfWork.RollbackTransactionAsync();
            }

            return Result.Failure<GetCourseResourceResponse>(HttpStatusCode.InternalServerError,
                CommonErrors.InternalServerError());
        }
    }
}
