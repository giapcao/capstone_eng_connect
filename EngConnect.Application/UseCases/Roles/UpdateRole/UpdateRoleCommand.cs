using System.Text.Json.Serialization;
using EngConnect.BuildingBlock.Application.Base;

namespace EngConnect.Application.UseCases.Roles.UpdateRole;

public class UpdateRoleCommand : ICommand
{
    [JsonIgnore]
    public Guid Id { get; set; }
    public required string Code { get; set; }
    public string? Description { get; set; }
}
