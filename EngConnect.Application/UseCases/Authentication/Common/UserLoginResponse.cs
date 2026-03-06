namespace EngConnect.Application.UseCases.Authentication.Common;

public record UserLoginResponse
{
    public string FirstName { get; init; } = null!;
    public string LastName { get; init; } = null!;
    public string AccessToken { get; init; } = null!;
    public string RefreshToken { get; init; } = null!;
}