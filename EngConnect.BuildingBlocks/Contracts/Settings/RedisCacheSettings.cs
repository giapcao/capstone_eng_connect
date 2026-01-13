namespace EngConnect.BuildingBlock.Contracts.Settings;

public class RedisCacheSettings
{
    public static readonly string Section = "RedisCacheSettings";
    public int RefreshTokenExpirationMinutes { get; set; }
    public int EmailVerificationTokenExpirationMinutes { get; set; }
    public int PasswordResetTokenExpirationMinutes { get; set; }
    public int EmailResetPasswordTokenExpirationInMinutes { get; set; }
    public int VerificationTotpCodeExpirationMinutes { get; set; }
    public int SettingCacheExpirationMinutes { get; set; }
    public int CollectionCacheExpirationMinutes { get; set; }
}