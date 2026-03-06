using System.Text.Json.Serialization;
using EngConnect.BuildingBlock.Application.Base;

namespace EngConnect.Application.UseCases.Categories.UpdateCategory;

public class UpdateCategoryCommand : ICommand
{
    [JsonIgnore]
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    public string? Type { get; set; }
}
