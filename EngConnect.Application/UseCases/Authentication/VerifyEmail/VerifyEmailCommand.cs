using EngConnect.BuildingBlock.Application.Base;

namespace EngConnect.Application.UseCases.Authentication.VerifyEmail;

public record VerifyEmailCommand: ICommand
{
    public string Token { get; set; } = null!;
}