using System.Text.Json.Serialization;
using EngConnect.BuildingBlock.Application.Base;

namespace EngConnect.Application.UseCases.Users.ChangePassword;

public record ChangePasswordCommand : ICommand
{
    [JsonIgnore] public Guid UserId { get; set; }
    public string OldPassword { get; init; } = null!;
    public string NewPassword { get; init; } = null!;
}

