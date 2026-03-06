namespace EngConnect.Application.UseCases.PermissionRoles.Common;

public class GetPermissionRoleResponse
{
    public Guid Id { get; set; }
    public Guid PermissionId { get; set; }
    public Guid RoleId { get; set; }
    public DateTime CreateAt { get; set; }
    public DateTime? UpdateAt { get; set; }
}
