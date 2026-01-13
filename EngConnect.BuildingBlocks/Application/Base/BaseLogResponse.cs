namespace EngConnect.BuildingBlock.Application.Base;

public record BaseLogResponse<TKey>
{
    public TKey Id { get; init; } = default!;
    public string BeforeUpdate { get; init; } = null!;
    public string AfterUpdate { get; init; } =  null!;
    public Guid? CreatedById { get; init; }
    public string? CreatedBy { get; init; }
    public DateTimeOffset UpdatedAt { get; init; }
}