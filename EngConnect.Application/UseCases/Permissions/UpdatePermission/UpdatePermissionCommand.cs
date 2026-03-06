using System.Text.Json.Serialization;
using EngConnect.BuildingBlock.Application.Base;

namespace EngConnect.Application.UseCases.Permissions.UpdatePermission;

public class UpdatePermissionCommand : ICommand
{
    [JsonIgnore]
    public Guid Id { get; set; }
    public required string Code { get; set; }
    public string? Description { get; set; }
}
