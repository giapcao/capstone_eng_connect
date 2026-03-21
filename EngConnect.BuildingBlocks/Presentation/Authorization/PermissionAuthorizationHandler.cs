using EngConnect.BuildingBlock.Domain.Constants;
using Microsoft.AspNetCore.Authorization;

namespace EngConnect.BuildingBlock.Presentation.Authorization;

public class PermissionAuthorizationHandler : AuthorizationHandler<PermissionRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
        PermissionRequirement requirement)
    {
        if (context.User.IsInRole(nameof(UserRoleEnum.Admin)))
        {
            context.Succeed(requirement);
            return Task.CompletedTask;
        }

        // Check if user has the required feature
        var userFeatures = context.User.Claims
            .Where(c => c.Type == "Permission")
            .Select(c => c.Value);

        if (userFeatures.Contains(requirement.Permission))
        {
            context.Succeed(requirement);
        }

        return Task.CompletedTask;
    }
}

public class PermissionRequirement : IAuthorizationRequirement
{
    public string Permission { get; }

    public PermissionRequirement(string permission)
    {
        Permission = permission;
    }
}