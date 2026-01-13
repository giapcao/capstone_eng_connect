namespace EngConnect.BuildingBlock.Domain.Constants;

public enum OutboxStatus
{
    Pending,
    Processing,
    Published,
    Failed,
    Dead
}