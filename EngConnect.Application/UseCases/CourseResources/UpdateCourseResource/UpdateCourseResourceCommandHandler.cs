using System.Net;
using EngConnect.Application.UseCases.CourseResources.Common;
using EngConnect.BuildingBlock.Application.Base;
using EngConnect.BuildingBlock.Contracts.Abstraction;
using EngConnect.BuildingBlock.Contracts.Shared;
using EngConnect.BuildingBlock.Domain.DomainErrors;
using EngConnect.Domain.Constants;
using EngConnect.Domain.DomainErrors;
using EngConnect.Domain.Persistence.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EngConnect.Application.UseCases.CourseResources.UpdateCourseResource;

public class UpdateCourseResourceCommandHandler : ICommandHandler<UpdateCourseResourceCommand, GetCourseResourceResponse>
{
    private readonly ILogger<UpdateCourseResourceCommandHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateCourseResourceCommandHandler(ILogger<UpdateCourseResourceCommandHandler> logger, IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<GetCourseResourceResponse>> HandleAsync(UpdateCourseResourceCommand command,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Start UpdateCourseResourceCommandHandler {@Command}", command);
        try
        {
            var courseResourceRepo = _unitOfWork.GetRepository<CourseResource, Guid>();

            var courseResource = await courseResourceRepo.FindAll(x => x.Id == command.Id)
                .Include(x => x.CourseSessionCourseResources)
                    .ThenInclude(x => x.CourseSession)
                        .ThenInclude(x => x.CourseModuleCourseSessions)
                            .ThenInclude(x => x.CourseModule)
                                .ThenInclude(x => x.CourseCourseModules)
                                    .ThenInclude(x => x.Course)
                .FirstOrDefaultAsync(cancellationToken);
            if (courseResource == null)
            {
                _logger.LogWarning("CourseResource not found with ID: {Id}", command.Id);
                return Result.Failure<GetCourseResourceResponse>(HttpStatusCode.NotFound,
                    new Error("CourseResourceNotFound", "Tai nguyen khong ton tai"));
            }

            var hasPublishedCourse = courseResource.CourseSessionCourseResources
                .SelectMany(x => x.CourseSession.CourseModuleCourseSessions)
                .SelectMany(x => x.CourseModule.CourseCourseModules)
                .Any(x => x.Course.Status == nameof(CourseStatus.Published));
            if (hasPublishedCourse)
            {
                _logger.LogWarning("CourseResource with ID: {Id} cannot be updated because it's in use by a Published course", command.Id);
                return Result.Failure<GetCourseResourceResponse>(HttpStatusCode.BadRequest,
                    CourseResourceErrors.CourseResourceIsInUse());
            }

            courseResource.Title = command.Title;
            courseResource.ResourceType = command.ResourceType;
            courseResource.Url = command.Url;
            courseResource.Status = command.Status;

            courseResourceRepo.Update(courseResource);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("End UpdateCourseResourceCommandHandler");
            return Result.Success(new GetCourseResourceResponse
            {
                Id = courseResource.Id,
                TutorId = courseResource.TutorId ?? Guid.Empty,
                Title = courseResource.Title,
                ResourceType = courseResource.ResourceType,
                Url = courseResource.Url,
                Status = courseResource.Status,
                CreatedAt = courseResource.CreatedAt,
                UpdatedAt = courseResource.UpdatedAt
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred in UpdateCourseResourceCommandHandler: {Message}", ex.Message);
            return Result.Failure<GetCourseResourceResponse>(HttpStatusCode.InternalServerError,
                CommonErrors.InternalServerError());
        }
    }
}
