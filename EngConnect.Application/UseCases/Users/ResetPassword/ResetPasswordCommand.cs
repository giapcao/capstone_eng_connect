using EngConnect.BuildingBlock.Application.Base;

namespace EngConnect.Application.UseCases.Users.ResetPassword;

public record ResetPasswordCommand: ICommand
{
    /// <summary>
    /// Token from email to reset password
    /// </summary>
    public string Token { get; init; } = null!;
    public string NewPassword { get; init; } = null!;
}