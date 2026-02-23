namespace EngConnect.BuildingBlock.Contracts.Settings;

/// <summary>
/// Configuration settings for redirect URLs.
/// </summary>
public class RedirectUrlSettings
{
    //OAuth Redirect URLs
    public static readonly string Section = "RedirectUrlSettings";
    
    /// <summary>
    /// URL to redirect to when Google login fails.
    /// </summary>
    public string GoogleLoginFailedUrl { get; set; } = null!;
    
    /// <summary>
    /// URL to redirect to when Google login is successful.
    /// </summary>
    public string GoogleLoginSuccessUrl { get; set; } = null!;
}