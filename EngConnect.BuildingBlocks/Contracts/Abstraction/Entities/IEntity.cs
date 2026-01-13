namespace EngConnect.BuildingBlock.Contracts.Abstraction.Entities;

public interface IEntity<TKey>
{
    TKey Id { get; set; }
}