namespace EngConnect.Application.UseCases.Roles.Common;

public record GetRoleResponse
{
    public Guid Id { get; set; }
    public string Code { get; set; } = null!;
    public string? Description { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
