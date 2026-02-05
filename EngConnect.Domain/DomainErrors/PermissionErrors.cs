using EngConnect.BuildingBlock.Contracts.Shared;

namespace EngConnect.Domain.DomainErrors;

public static class PermissionErrors
{
    public static Error PermissionNotFound () =>
        new("Permission.PermissionNotFound", "Quyền không tồn tại.");
    
    public static Error PermissionAlreadyExists () =>
        new("Permission.PermissionAlreadyExists", "Quyền đã tồn tại.");
}