using EngConnect.BuildingBlock.Application.Base;

namespace EngConnect.Application.UseCases.Categories.DeleteCategory;

public record DeleteCategoryCommand(Guid Id) : ICommand;
