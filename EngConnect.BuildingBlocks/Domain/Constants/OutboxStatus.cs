namespace EngConnect.BuildingBlock.Domain.Constants;

public enum OutboxStatus
{
    Pending,
    Processing,     //Being processed, lock event
    Published,      //Sent successfully
    Failed,         //Sending failed
    Dead            //After max retries, mark as dead
}