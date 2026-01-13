using EngConnect.BuildingBlock.Domain.Constants;

namespace EngConnect.BuildingBlock.Domain.Abstraction;

public interface IEntityVisibilityPolicy
{
    /// <summary>
    ///     <para>Gets the integer values of the statuses that should be visible for a given entity and user role.</para>
    ///     <para>A null value for userRole represents an anonymous user.</para>
    ///     <para>This requires implementation to explicitly define the visible statuses for each entity and user role.</para>
    /// </summary>
    /// <returns>A collection of integers, or null if no filtering should be applied.</returns>
    IReadOnlyCollection<int> GetVisibleStatuses<T>(UserRole? userRole);
}