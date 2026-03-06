using System.Net;
using EngConnect.BuildingBlock.Application.Base;
using EngConnect.BuildingBlock.Contracts.Abstraction;
using EngConnect.BuildingBlock.Contracts.Shared;
using EngConnect.BuildingBlock.Domain.DomainErrors;
using EngConnect.Domain.Persistence.Models;
using Microsoft.Extensions.Logging;

namespace EngConnect.Application.UseCases.Categories.CreateCategory;

public class CreateCategoryCommandHandler : ICommandHandler<CreateCategoryCommand>
{
    private readonly ILogger<CreateCategoryCommandHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public CreateCategoryCommandHandler(ILogger<CreateCategoryCommandHandler> logger, IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> HandleAsync(CreateCategoryCommand command, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Start CreateCategoryCommandHandler {@Command}", command);
        try
        {
            var categoryRepo = _unitOfWork.GetRepository<Category, Guid>();

            // Check if category name already exists
            var categoryExists = await categoryRepo.AnyAsync(
                x => x.Name == command.Name && x.Type == command.Type, 
                cancellationToken);
            if (categoryExists)
            {
                _logger.LogWarning("Category already exists with name: {Name}", command.Name);
                return Result.Failure(HttpStatusCode.BadRequest, new Error("CategoryExists", "Danh mục đã tồn tại"));
            }

            var category = new Category
            {
                Name = command.Name,
                Description = command.Description,
                Type = command.Type
            };

            categoryRepo.Add(category);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("End CreateCategoryCommandHandler");
            return Result.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred in CreateCategoryCommandHandler: {Message}", ex.Message);
            return Result.Failure(HttpStatusCode.InternalServerError, CommonErrors.InternalServerError());
        }
    }
}
