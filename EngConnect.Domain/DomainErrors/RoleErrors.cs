using EngConnect.BuildingBlock.Contracts.Shared;

namespace EngConnect.Domain.DomainErrors;

public static class RoleErrors
{
    public static Error RoleAlreadyExists() =>
        new("Role.RoleAlreadyExists", "Vai trò đã tồn tại.");
    
    public static Error RoleNotFound() =>
        new("Role.RoleNotFound", "Vai trò không tồn tại.");
}