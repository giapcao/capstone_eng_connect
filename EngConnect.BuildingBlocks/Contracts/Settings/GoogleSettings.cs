namespace EngConnect.BuildingBlock.Contracts.Settings;

public sealed class GoogleSettings
{
    public const string Section = "GoogleSettings";
    public string ClientId { get; set; } = null!;
    public string ClientSecret { get; set; } = null!;
}