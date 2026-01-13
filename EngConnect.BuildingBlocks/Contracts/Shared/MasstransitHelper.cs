using System.Collections.Concurrent;
using System.Reflection;
using EngConnect.BuildingBlock.EventBus.Abstraction;

namespace EngConnect.BuildingBlock.Contracts.Shared;

public static class MasstransitHelper
{
    // Anchor assembly safely
    private static readonly Assembly EventAssembly = typeof(EventBase).Assembly;

    private static readonly ConcurrentDictionary<string, Type?> Cache = new();

    /// <summary>
    /// Resolve event type with caching
    /// </summary>
    public static Type? GetEventTypeWithCache(string eventTypeName)
    {
        if (string.IsNullOrWhiteSpace(eventTypeName))
            return null;

        return Cache.GetOrAdd(eventTypeName, ResolveEventType);
    }

    private static Type? ResolveEventType(string eventTypeName)
    {
        // Full type name (Namespace.Class)
        var type = Type.GetType(eventTypeName);
        if (type != null)
            return type;

        // Fallback: class name only
        return EventAssembly
            .GetTypes().FirstOrDefault(t =>
                t.IsClass && !t.IsAbstract && t.Name.Equals(eventTypeName, StringComparison.Ordinal));
    }

    /// <summary>
    /// Validate MassTransit-compatible event type
    /// </summary>
    public static bool IsValidEventType(Type? type)
    {
        if (type == null)
            return false;

        if (!type.IsClass || type.IsAbstract)
            return false;

        // Assembly guard
        if (type.Assembly != EventAssembly)
            return false;

        return true;
    }

    /// <summary>
    /// Resolve and validate event type
    /// </summary>
    public static Type? GetValidEventType(string eventTypeName)
    {
        var type = GetEventTypeWithCache(eventTypeName);
        return IsValidEventType(type) ? type : null;
    }

    public static void ClearCache() => Cache.Clear();
}