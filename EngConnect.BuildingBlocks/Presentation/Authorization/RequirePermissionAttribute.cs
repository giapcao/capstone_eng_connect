using Microsoft.AspNetCore.Authorization;

namespace EngConnect.BuildingBlock.Presentation.Authorization;

/// <summary>
///     Require a specific feature to access the endpoint (Admin role is always allowed)
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
public class RequirePermissionAttribute : AuthorizeAttribute
{
    public RequirePermissionAttribute(string permission) : base(policy: permission)
    {
    }
}