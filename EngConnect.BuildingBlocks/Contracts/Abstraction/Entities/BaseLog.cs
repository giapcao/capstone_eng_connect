namespace EngConnect.BuildingBlock.Contracts.Abstraction.Entities;

public abstract class BaseLog : BaseEntity<int>
{
    public required string BeforeUpdate { get; set; }
    public required string AfterUpdate { get; set; }
    public Guid? CreatedById { get; set; }
    public string? CreatedBy { get; set; }
    public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.UtcNow;
}