namespace EngConnect.BuildingBlock.Application.Base;

/// <summary>
///     Interface for queries that filter by status
/// </summary>
/// <typeparam name="TEnum">The enum type used for status filtering</typeparam>
public interface IStatusFilterable<TEnum> where TEnum : struct, Enum
{
    public TEnum[] Statuses { get; set; }
}