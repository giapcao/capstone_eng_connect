using MassTransit;

namespace EngConnect.BuildingBlock.EventBus.Abstraction;

[ExcludeFromTopology]
public abstract class EventBase : IEvent
{
    protected EventBase()
    {
        IdempotencyId = Guid.NewGuid();
        EventType = GetType().Name;
    }

    public Guid IdempotencyId { get; init; }
    public string EventType { get; }
}