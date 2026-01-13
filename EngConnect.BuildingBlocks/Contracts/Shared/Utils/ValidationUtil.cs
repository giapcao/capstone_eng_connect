using System.Diagnostics.CodeAnalysis;

namespace EngConnect.BuildingBlock.Contracts.Shared.Utils;

public static class ValidationUtil
{
    /// <summary>
    /// Returns true if the given value is null or logically "empty".
    /// Supports strings, arrays, collections, and general enumerables.
    /// </summary>
    public static bool IsNullOrEmpty([NotNullWhen(false)] object? obj)
    {
        if (obj is null)
        {
            return true;
        }

        if (obj is string s)
        {
            return string.IsNullOrWhiteSpace(s);
        }

        if (obj is System.Collections.ICollection collection)
        {
            return collection.Count == 0;
        }

        if (obj is System.Collections.IEnumerable enumerable)
        {
            var enumerator = enumerable.GetEnumerator();
            try
            {
                var hasItem = enumerator.MoveNext();
                return hasItem == false;
            }
            finally
            {
                if (enumerator is IDisposable disposable)
                {
                    disposable.Dispose();
                }
            }
        }

        return false;
    }

    public static bool IsNotNullOrEmpty([NotNullWhen(true)] object? obj)
    {
        return !IsNullOrEmpty(obj);
    }
}