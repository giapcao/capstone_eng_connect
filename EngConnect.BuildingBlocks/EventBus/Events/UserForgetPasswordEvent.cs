using EngConnect.BuildingBlock.EventBus.Abstraction;

namespace EngConnect.BuildingBlock.EventBus.Events;

/// <summary>
///     Event published when a user requests to reset their password
/// </summary>
public class UserForgetPasswordEvent : NotificationEvent
{
    /// <summary>
    ///     The password reset token
    /// </summary>
    public string ResetToken { get; set; } = null!;

    /// <summary>
    ///     The full name of the user
    /// </summary>
    public string FullName { get; set; } = null!;

    /// <summary>
    ///     The username of the user
    /// </summary>
    public string Username { get; set; } = null!;
}
