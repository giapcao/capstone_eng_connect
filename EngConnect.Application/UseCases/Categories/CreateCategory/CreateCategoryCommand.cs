using EngConnect.BuildingBlock.Application.Base;

namespace EngConnect.Application.UseCases.Categories.CreateCategory;

public class CreateCategoryCommand : ICommand
{
    public required string Name { get; set; }
    public string? Description { get; set; }
    public string? Type { get; set; }
}
