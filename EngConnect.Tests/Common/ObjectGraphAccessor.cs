using System.Reflection;

namespace EngConnect.Tests.Common;

internal static class ObjectGraphAccessor
{
    public static void SetValue(object root, string propertyPath, object? value)
    {
        var segments = propertyPath.Split('.', StringSplitOptions.RemoveEmptyEntries);
        object current = root;

        for (var index = 0; index < segments.Length - 1; index++)
        {
            var property = current.GetType().GetProperty(segments[index], BindingFlags.Instance | BindingFlags.Public)
                           ?? throw new InvalidOperationException($"Property {segments[index]} was not found on {current.GetType().FullName}.");

            var next = property.GetValue(current);
            if (next == null)
            {
                next = TestObjectFactory.CreateValue(property.PropertyType)
                       ?? throw new InvalidOperationException($"Cannot create {property.PropertyType.FullName}.");
                property.SetValue(current, next);
            }

            current = next;
        }

        var targetProperty = current.GetType().GetProperty(segments[^1], BindingFlags.Instance | BindingFlags.Public)
                            ?? throw new InvalidOperationException($"Property {segments[^1]} was not found on {current.GetType().FullName}.");

        targetProperty.SetValue(current, value);
    }
}
