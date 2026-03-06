using EngConnect.Application.UseCases.Authentication.Common;
using EngConnect.BuildingBlock.Application.Base;

namespace EngConnect.Application.UseCases.Authentication.LoginByUser;

public record LoginByUserCommand: ICommand<UserLoginResponse>
{
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
}