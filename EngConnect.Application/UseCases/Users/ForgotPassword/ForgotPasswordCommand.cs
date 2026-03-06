using EngConnect.BuildingBlock.Application.Base;

namespace EngConnect.Application.UseCases.Users.ForgotPassword;

public record ForgotPasswordCommand: ICommand
{
    public string Email { get; set; } = null!;
}