using MassTransit;

namespace EngConnect.BuildingBlock.EventBus.Abstraction;

// Add [ExcludeFromTopology] to prevent MassTransit from creating exchange for this interface
[ExcludeFromTopology]
public interface IEvent
{
    Guid IdempotencyId { get; }
    string EventType { get; }
}