namespace EngConnect.Application.UseCases.UserRoles.Common;

public class GetUserRoleResponse
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid RoleId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdateAt { get; set; }
}
