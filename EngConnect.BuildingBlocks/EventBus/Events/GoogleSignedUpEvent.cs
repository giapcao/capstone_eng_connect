using EngConnect.BuildingBlock.EventBus.Abstraction;

namespace EngConnect.BuildingBlock.EventBus.Events;

/// <summary>
///     Event published when a new user signs up using Google OAuth
/// </summary>
public class GoogleSignedUpEvent : NotificationEvent
{
    /// <summary>
    ///     The auto-generated password for the user
    /// </summary>
    public string GeneratedPassword { get; set; } = null!;

    /// <summary>
    ///     The full name of the user
    /// </summary>
    public string FullName { get; set; } = null!;

    /// <summary>
    ///     The username of the user
    /// </summary>
    public string Username { get; set; } = null!;
}
