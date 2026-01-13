using MassTransit;

namespace EngConnect.BuildingBlock.EventBus.Abstraction;

[ExcludeFromTopology]
public abstract class NotificationEvent : EventBase
{
    /// <summary>
    ///     Id of the issuer of the notification
    /// </summary>
    public Guid IssuerId { get; set; }

    /// <summary>
    ///    Channel through which the notification is sent (e.g., Email, In-app, ...)
    /// </summary>
    public string Channel { get; set; } = null!;

    /// <summary>
    ///     Optional resource id that the notification is related to
    /// </summary>
    public string? ResourceId { get; set; }

    /// <summary>
    ///     Optional resource type that the notification is related to
    /// </summary>
    public string? ResourceType { get; set; }

    /// <summary>
    ///     List of recipient ids
    /// </summary>
    public Guid[] RecipientId { get; set; } = [];

    /// <summary>
    ///     The target audiences for this notification (user roles)
    /// </summary>
    public string[] Audiences { get; set; } = [];
}