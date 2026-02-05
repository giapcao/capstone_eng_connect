using EngConnect.BuildingBlock.Contracts.Shared;

namespace EngConnect.Domain.DomainErrors;

public static class UserRoleErrors
{
    public static Error UserRoleAlreadyExists () =>
        new("UserRole.UserRoleAlreadyExists", "Người dùng đã có vai trò này.");
    
    public static Error UserRoleNotFound () =>
        new("UserRole.UserRoleNotFound", "Vai trò của người dùng không tồn tại .");
}