using EngConnect.Application.UseCases.Roles.Common;

namespace EngConnect.Application.UseCases.Users.Common;

public record GetUserResponse
{
    public Guid Id { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? UserName { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public string? AddressNum { get; set; }
    public string? ProvinceId { get; set; }
    public string? ProvinceName { get; set; }
    public string? WardId { get; set; }
    public string? WardName { get; set; }
    public string? Status { get; set; }
    public List<GetRoleForUserResponse>? Roles { get; set; } 
    public bool? IsEmailVerified { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public record GetRoleForUserResponse
{
    public Guid Id { get; set; }
    public string Code { get; set; } = null!;
    public string? Description { get; set; }
    public List<GetPermissionResponseForUser>? Permissions { get; set; }
}

public record GetPermissionResponseForUser
{
    public Guid Id { get; set; }
    public string Code { get; set; } = null!;
    public string? Description { get; set; }
}
