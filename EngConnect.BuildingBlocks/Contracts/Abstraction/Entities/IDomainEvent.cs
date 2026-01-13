namespace EngConnect.BuildingBlock.Contracts.Abstraction.Entities;

public interface IDomainEvent
{
    DateTime Timestamp { get; }
}