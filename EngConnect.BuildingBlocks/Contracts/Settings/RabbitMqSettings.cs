namespace EngConnect.BuildingBlock.Contracts.Settings;

public class RabbitMqSettings
{
    public const string Section = "RabbitMqSettings";
    
    public string Host { get; set; } = null!;
    public string VirtualHost { get; set; } = "/";
    public string Username { get; set; } = null!;
    public string Password { get; set; } = null!;
}