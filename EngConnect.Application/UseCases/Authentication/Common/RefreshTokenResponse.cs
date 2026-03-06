namespace EngConnect.Application.UseCases.Authentication.Common;

public record RefreshTokenResponse
{
    public string AccessToken { get; set; } = null!;
    public string RefreshToken { get; set; } = null!;
}