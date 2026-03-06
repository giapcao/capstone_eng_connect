using EngConnect.BuildingBlock.Contracts.Shared;

namespace EngConnect.Domain.DomainErrors;

public static class PermissionRoleErrors
{
    public static Error PermissionRoleNotFound() =>
        new("PermissionRole.PermissionRoleNotFound", "Vai trò không có quyền này.");
    
    public static Error PermissionRoleAlreadyExists() =>
        new("PermissionRole.PermissionRoleAlreadyExists", "Vai trò đã có quyền này.");
}