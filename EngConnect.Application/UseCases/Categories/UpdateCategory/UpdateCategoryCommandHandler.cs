using System.Net;
using EngConnect.BuildingBlock.Application.Base;
using EngConnect.BuildingBlock.Contracts.Abstraction;
using EngConnect.BuildingBlock.Contracts.Shared;
using EngConnect.BuildingBlock.Domain.DomainErrors;
using EngConnect.Domain.Persistence.Models;
using Microsoft.Extensions.Logging;

namespace EngConnect.Application.UseCases.Categories.UpdateCategory;

public class UpdateCategoryCommandHandler : ICommandHandler<UpdateCategoryCommand>
{
    private readonly ILogger<UpdateCategoryCommandHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateCategoryCommandHandler(ILogger<UpdateCategoryCommandHandler> logger, IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> HandleAsync(UpdateCategoryCommand command, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Start UpdateCategoryCommandHandler {@Command}", command);
        try
        {
            var categoryRepo = _unitOfWork.GetRepository<Category, Guid>();

            var category = await categoryRepo.FindSingleAsync(
                x => x.Id == command.Id,
                cancellationToken: cancellationToken);
            if (category == null)
            {
                _logger.LogWarning("Category not found with ID: {Id}", command.Id);
                return Result.Failure(HttpStatusCode.NotFound, new Error("CategoryNotFound", "Danh mục không tồn tại"));
            }

            // Check if new name already exists (except current category)
            var nameExists = await categoryRepo.AnyAsync(
                x => x.Name == command.Name && x.Type == command.Type && x.Id != command.Id, 
                cancellationToken);
            if (nameExists)
            {
                _logger.LogWarning("Category name already exists: {Name}", command.Name);
                return Result.Failure(HttpStatusCode.BadRequest, new Error("CategoryNameExists", "Tên danh mục đã tồn tại"));
            }

            category.Name = command.Name;
            category.Description = command.Description;
            category.Type = command.Type;
            categoryRepo.Update(category);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("End UpdateCategoryCommandHandler");
            return Result.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred in UpdateCategoryCommandHandler: {Message}", ex.Message);
            return Result.Failure(HttpStatusCode.InternalServerError, CommonErrors.InternalServerError());
        }
    }
}
