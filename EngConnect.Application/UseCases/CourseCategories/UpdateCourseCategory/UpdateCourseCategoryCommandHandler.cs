using System.Net;
using EngConnect.BuildingBlock.Application.Base;
using EngConnect.BuildingBlock.Contracts.Abstraction;
using EngConnect.BuildingBlock.Contracts.Shared;
using EngConnect.BuildingBlock.Domain.DomainErrors;
using EngConnect.Domain.Persistence.Models;
using Microsoft.Extensions.Logging;

namespace EngConnect.Application.UseCases.CourseCategories.UpdateCourseCategory;

public class UpdateCourseCategoryCommandHandler : ICommandHandler<UpdateCourseCategoryCommand>
{
    private readonly ILogger<UpdateCourseCategoryCommandHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateCourseCategoryCommandHandler(ILogger<UpdateCourseCategoryCommandHandler> logger, IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> HandleAsync(UpdateCourseCategoryCommand command, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Start UpdateCourseCategoryCommandHandler {@Command}", command);
        try
        {
            var courseCategoryRepo = _unitOfWork.GetRepository<CourseCategory, Guid>();
            var categoryRepo = _unitOfWork.GetRepository<Category, Guid>();

            var courseCategory = await courseCategoryRepo.FindSingleAsync(
                x => x.Id == command.Id,
                cancellationToken: cancellationToken);
            if (courseCategory == null)
            {
                _logger.LogWarning("CourseCategory not found with ID: {Id}", command.Id);
                return Result.Failure(HttpStatusCode.NotFound, new Error("CourseCategoryNotFound", "CourseCategory không tồn tại"));
            }

            // Check if category exists
            var categoryExists = await categoryRepo.AnyAsync(x => x.Id == command.CategoryId, cancellationToken);
            if (!categoryExists)
            {
                _logger.LogWarning("Category not found with ID: {CategoryId}", command.CategoryId);
                return Result.Failure(HttpStatusCode.NotFound, new Error("CategoryNotFound", "Danh mục không tồn tại"));
            }

            courseCategory.CategoryId = command.CategoryId;
            courseCategoryRepo.Update(courseCategory);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("End UpdateCourseCategoryCommandHandler");
            return Result.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred in UpdateCourseCategoryCommandHandler: {Message}", ex.Message);
            return Result.Failure(HttpStatusCode.InternalServerError, CommonErrors.InternalServerError());
        }
    }
}
