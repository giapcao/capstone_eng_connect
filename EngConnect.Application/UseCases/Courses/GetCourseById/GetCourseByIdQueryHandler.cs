using System.Net;
using EngConnect.Application.UseCases.Courses.Common;
using EngConnect.BuildingBlock.Application.Base;
using EngConnect.BuildingBlock.Contracts.Abstraction;
using EngConnect.BuildingBlock.Contracts.Shared;
using EngConnect.BuildingBlock.Contracts.Shared.Utils;
using EngConnect.BuildingBlock.Domain.DomainErrors;
using EngConnect.Domain.Persistence.Models;
using Mapster;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EngConnect.Application.UseCases.Courses.GetCourseById;

public class GetCourseByIdQueryHandler : IQueryHandler<GetCourseByIdQuery, GetCourseResponseDetail>
{
    private readonly ILogger<GetCourseByIdQueryHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IAwsStorageService _awsStorageService;

    public GetCourseByIdQueryHandler(ILogger<GetCourseByIdQueryHandler> logger, IUnitOfWork unitOfWork, IMapper mapper, IAwsStorageService awsStorageService)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _awsStorageService = awsStorageService;
    }

    public async Task<Result<GetCourseResponseDetail>> HandleAsync(GetCourseByIdQuery query,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Start GetCourseByIdQueryHandler {@Query}", query);
        try
        {
            var courseRepo = _unitOfWork.GetRepository<Course, Guid>();

            var course = await courseRepo.FindAll(
                    x => x.Id == query.Id,
                    tracking: false,
                    cancellationToken: cancellationToken)
                .Include(x => x.CourseCategories)
                .ThenInclude(x => x.Category)
                .Include(x => x.CourseCourseModules)
                .ThenInclude(x => x.CourseModule)
                .ThenInclude(x => x.CourseModuleCourseSessions)
                .ThenInclude(x => x.CourseSession)
                .ProjectToType<GetCourseResponseDetail>()
                .FirstOrDefaultAsync(cancellationToken);

            if (ValidationUtil.IsNullOrEmpty(course))
            {
                _logger.LogWarning("Course not found with ID: {Id}", query.Id);
                return Result.Failure<GetCourseResponseDetail>(HttpStatusCode.BadRequest,
                    new Error("CourseNotFound", "Khóa học không tồn tại"));
            }

            // Convert relative paths to full AWS S3 URLs
            course.ThumbnailUrl = course.ThumbnailUrl != null ? _awsStorageService.GetFileUrl(course.ThumbnailUrl) : null;
            course.DemoVideoUrl = course.DemoVideoUrl != null ? _awsStorageService.GetFileUrl(course.DemoVideoUrl) : null;

            _logger.LogInformation("End GetCourseByIdQueryHandler");
            return Result.Success(course);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred in GetCourseByIdQueryHandler: {Message}", ex.Message);
            return Result.Failure<GetCourseResponseDetail>(HttpStatusCode.InternalServerError,
                CommonErrors.InternalServerError());
        }
    }
}