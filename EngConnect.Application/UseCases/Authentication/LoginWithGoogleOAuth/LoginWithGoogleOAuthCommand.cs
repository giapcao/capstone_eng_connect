using System.Security.Claims;
using EngConnect.BuildingBlock.Application.Base;

namespace EngConnect.Application.UseCases.Authentication.LoginWithGoogleOAuth;

/// <summary>
/// Login by Google OAuth
/// Return redirect URL
/// </summary>
public class LoginWithGoogleOAuthCommand: ICommand<string>
{
    public ClaimsPrincipal? Principal { get; set; }
}