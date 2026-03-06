namespace EngConnect.Application.UseCases.Users.Common;

public class GetUserResponseDetail: GetUserResponse
{
    public List<GetRoleForUserResponse>? UserRoles { get; set; } 
}

public class GetRoleForUserResponse
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid RoleId { get; set; }
    public string RoleCode { get; set; } = null!;
    public string? RoleDescription { get; set; }
    public List<GetPermissionResponseForUser>? PermissionRoles { get; set; }
}

public class GetPermissionResponseForUser
{
    public Guid Id { get; set; }
    public Guid RoleId { get; set; }
    public Guid PermissionId { get; set; }
    public string PermissionCode { get; set; } = null!;
    public string? PermissionDescription { get; set; }
}
