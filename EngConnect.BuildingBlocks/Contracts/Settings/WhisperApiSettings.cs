namespace EngConnect.BuildingBlock.Contracts.Settings;

public class WhisperApiSettings
{
    public const string Section = "WhisperEndpoint";

    public string Endpoint { get; set; } = null!;
}