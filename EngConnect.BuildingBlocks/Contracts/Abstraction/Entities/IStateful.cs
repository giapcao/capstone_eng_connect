namespace EngConnect.BuildingBlock.Contracts.Abstraction.Entities;

public interface IStateful<out TEnum> where TEnum : struct, Enum
{
    TEnum Status { get; }
}