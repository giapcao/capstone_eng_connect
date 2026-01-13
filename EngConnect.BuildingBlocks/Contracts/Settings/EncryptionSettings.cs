namespace EngConnect.BuildingBlock.Contracts.Settings;

public class EncryptionSettings
{
    public static readonly string Section = "EncryptionSettings";
    public string Key { get; set; } = null!;
    // ReSharper disable once InconsistentNaming
    public string IV { get; set; } = null!;
}