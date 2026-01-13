using EngConnect.BuildingBlock.EventBus.Abstraction;

namespace EngConnect.BuildingBlock.EventBus.Utils;

/// <summary>
///     Helper utilities for working with notification events
/// </summary>
public static class NotificationHelper
{
    /// <summary>
    ///     Creates a notification event with recipient-specific properties and multiple roles
    /// </summary>
    /// <typeparam name="T">Type of notification event</typeparam>
    /// <param name="baseEvent">Base event with common properties</param>
    /// <param name="recipientId">ID of the recipient</param>
    /// <param name="audiences">Roles of the recipient</param>
    /// <param name="channel">Notification channel</param>
    /// <returns>A new notification event with recipient-specific properties</returns>
    public static T CreateNotification<T>(T baseEvent, Guid[] recipientId, string[] audiences, string channel)
        where T : NotificationEvent, new()
    {
        var notification = new T();
        foreach (var prop in typeof(T).GetProperties())
        {
            if (prop is { CanWrite: true, CanRead: true })
            {
                prop.SetValue(notification, prop.GetValue(baseEvent));
            }
        }

        notification.RecipientId = recipientId;
        notification.Audiences = audiences;
        notification.Channel = channel;

        return notification;
    }
}