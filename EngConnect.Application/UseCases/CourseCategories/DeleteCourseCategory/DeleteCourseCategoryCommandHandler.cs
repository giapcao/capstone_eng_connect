using System.Net;
using EngConnect.BuildingBlock.Application.Base;
using EngConnect.BuildingBlock.Contracts.Abstraction;
using EngConnect.BuildingBlock.Contracts.Shared;
using EngConnect.BuildingBlock.Domain.DomainErrors;
using EngConnect.Domain.Persistence.Models;
using Microsoft.Extensions.Logging;

namespace EngConnect.Application.UseCases.CourseCategories.DeleteCourseCategory;

public class DeleteCourseCategoryCommandHandler : ICommandHandler<DeleteCourseCategoryCommand>
{
    private readonly ILogger<DeleteCourseCategoryCommandHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteCourseCategoryCommandHandler(ILogger<DeleteCourseCategoryCommandHandler> logger, IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> HandleAsync(DeleteCourseCategoryCommand command, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Start DeleteCourseCategoryCommandHandler {@Command}", command);
        try
        {
            var courseCategoryRepo = _unitOfWork.GetRepository<CourseCategory, Guid>();

            var courseCategory = await courseCategoryRepo.FindSingleAsync(
                x => x.Id == command.Id,
                cancellationToken: cancellationToken);
            if (courseCategory == null)
            {
                _logger.LogWarning("CourseCategory not found with ID: {Id}", command.Id);
                return Result.Failure(HttpStatusCode.NotFound, new Error("CourseCategoryNotFound", "CourseCategory không tồn tại"));
            }

            courseCategoryRepo.Delete(courseCategory);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("End DeleteCourseCategoryCommandHandler");
            return Result.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred in DeleteCourseCategoryCommandHandler: {Message}", ex.Message);
            return Result.Failure(HttpStatusCode.InternalServerError, CommonErrors.InternalServerError());
        }
    }
}
