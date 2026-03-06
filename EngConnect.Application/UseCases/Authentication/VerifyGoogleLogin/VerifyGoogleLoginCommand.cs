using EngConnect.Application.UseCases.Authentication.Common;
using EngConnect.BuildingBlock.Application.Base;

namespace EngConnect.Application.UseCases.Authentication.VerifyGoogleLogin;

public record VerifyGoogleLoginCommand: ICommand<UserLoginResponse>
{
    public string Token { get; set; } = null!;
}