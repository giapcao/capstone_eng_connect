using System.Net;
using EngConnect.BuildingBlock.Application.Base;
using EngConnect.BuildingBlock.Contracts.Abstraction;
using EngConnect.BuildingBlock.Contracts.Shared;
using EngConnect.BuildingBlock.Domain.DomainErrors;
using EngConnect.Domain.Persistence.Models;
using Microsoft.Extensions.Logging;

namespace EngConnect.Application.UseCases.CourseCategories.CreateCourseCategory;

public class CreateCourseCategoryCommandHandler : ICommandHandler<CreateCourseCategoryCommand>
{
    private readonly ILogger<CreateCourseCategoryCommandHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public CreateCourseCategoryCommandHandler(ILogger<CreateCourseCategoryCommandHandler> logger, IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> HandleAsync(CreateCourseCategoryCommand command, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Start CreateCourseCategoryCommandHandler {@Command}", command);
        try
        {
            var courseCategoryRepo = _unitOfWork.GetRepository<CourseCategory, Guid>();
            var courseRepo = _unitOfWork.GetRepository<Course, Guid>();
            var categoryRepo = _unitOfWork.GetRepository<Category, Guid>();

            // Check if course exists
            var courseExists = await courseRepo.AnyAsync(x => x.Id == command.CourseId, cancellationToken);
            if (!courseExists)
            {
                _logger.LogWarning("Course not found with ID: {CourseId}", command.CourseId);
                return Result.Failure(HttpStatusCode.NotFound, new Error("CourseNotFound", "Khóa học không tồn tại"));
            }

            // Check if category exists
            var categoryExists = await categoryRepo.AnyAsync(x => x.Id == command.CategoryId, cancellationToken);
            if (!categoryExists)
            {
                _logger.LogWarning("Category not found with ID: {CategoryId}", command.CategoryId);
                return Result.Failure(HttpStatusCode.NotFound, new Error("CategoryNotFound", "Danh mục không tồn tại"));
            }

            // Check if course category already exists
            var courseCategoryExists = await courseCategoryRepo.AnyAsync(
                x => x.CourseId == command.CourseId && x.CategoryId == command.CategoryId,
                cancellationToken);
            if (courseCategoryExists)
            {
                _logger.LogWarning("CourseCategory already exists for Course: {CourseId} and Category: {CategoryId}", command.CourseId, command.CategoryId);
                return Result.Failure(HttpStatusCode.BadRequest, new Error("CourseCategoryExists", "Khóa học đã có danh mục này"));
            }

            var courseCategory = new CourseCategory
            {
                CourseId = command.CourseId,
                CategoryId = command.CategoryId
            };

            courseCategoryRepo.Add(courseCategory);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("End CreateCourseCategoryCommandHandler");
            return Result.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred in CreateCourseCategoryCommandHandler: {Message}", ex.Message);
            return Result.Failure(HttpStatusCode.InternalServerError, CommonErrors.InternalServerError());
        }
    }
}
