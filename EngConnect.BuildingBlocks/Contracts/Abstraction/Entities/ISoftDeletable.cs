namespace EngConnect.BuildingBlock.Contracts.Abstraction.Entities;

public interface ISoftDeletable
{
    public bool IsDeleted { get; set; }
    public DateTime? DeletedAt { get; set; }
}