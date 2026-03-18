namespace EngConnect.BuildingBlock.Contracts.Settings;

public class GitHubModelsSettings
{
    public const string Section = "GitHubModels";
    public string Token { get; set; } = string.Empty;
    public string ModelName { get; set; } = string.Empty;
    public string Endpoint { get; set; } = string.Empty;
}