using EngConnect.BuildingBlock.Application.Base;

namespace EngConnect.Application.UseCases.Authentication.Logout;

public record LogoutCommand: ICommand
{
    public string AccessToken { get; set; } = null!;    
    //Get access token from request header, use for remove refresh token in db
}