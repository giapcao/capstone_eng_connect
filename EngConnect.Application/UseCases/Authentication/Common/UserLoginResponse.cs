namespace EngConnect.Application.UseCases.Authentication.Common;

public record UserLoginResponse
{
    public string FirstName { get; init; } = null!;
    public string LastName { get; init; } = null!;
    public string Username { get; init; } = null!;
    public List<string> Roles { get; init; } = [];
    public string? AvatarUrl { get; init; }
    public string AccessToken { get; init; } = null!;
    public string RefreshToken { get; init; } = null!;
}