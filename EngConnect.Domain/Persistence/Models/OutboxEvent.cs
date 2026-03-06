using System.Text.Json;
using EngConnect.BuildingBlock.Domain.Abstraction.Entities;
using EngConnect.BuildingBlock.Domain.Constants;

namespace EngConnect.Domain.Persistence.Models;

/// <summary>
/// Outbox event for event publishing
/// Stores events in the database before publishing to ensure transactional consistency
/// </summary>
public class OutboxEvent : TransactionalOutboxPattern
{
    //Constructor
    private OutboxEvent()
    {
        
    }
    private OutboxEvent(string aggregateType, Guid aggregateId, string eventType, string eventData)
    {
        this.AggregateType = aggregateType;
        this.AggregateId = aggregateId;
        this.EventType = eventType;
        this.EventData = eventData;
    }   
    
    //Create Event
    public static OutboxEvent CreateOutboxEvent (string aggregateType, Guid aggregateId, 
        object eventData)
    {
        var serializeData =  JsonSerializer.Serialize(eventData);
        return new OutboxEvent(aggregateType, aggregateId, eventData.GetType().FullName!, serializeData);
    }
    
    //Mark status
    public void MarkAsProcessing(Guid workerId)
    {
        OutboxStatus = OutboxStatus.Processing;
        LockBy = workerId;
        LockAt = DateTime.UtcNow;
        ProcessedAt = DateTime.UtcNow;
    }
    
    public void MarkAsPublished()
    {
        OutboxStatus = OutboxStatus.Published;
        SentAt = DateTime.UtcNow;
        LockAt = null;
        LockBy = null;
    }
    
    public void MarkAsFailed(int retryCount,  DateTime nextRetryAt ,string errorMessage)
    {
        OutboxStatus = OutboxStatus.Failed;
        FailedAt = DateTime.UtcNow;
        LastError = errorMessage;
        RetryCount += retryCount;
        NextRetryAt = nextRetryAt;
        LockAt = null;
        LockBy = null;
    }

    public void MarkAsDead()
    {
        OutboxStatus = OutboxStatus.Dead;
        DeadAt = DateTime.UtcNow;
        LockAt = null;
        LockBy = null;
    }
}
