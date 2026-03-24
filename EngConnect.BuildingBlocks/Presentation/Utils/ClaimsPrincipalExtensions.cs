using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using EngConnect.BuildingBlock.Application.Base;
using EngConnect.BuildingBlock.Domain.Constants;
using Microsoft.AspNetCore.Http;

namespace EngConnect.BuildingBlock.Presentation.Utils;

public static class ClaimsPrincipalExtensions
{
    public static bool IsAdmin(this ClaimsPrincipal? principal)
    {
        return principal != null && principal.IsInRole(nameof(UserRoleEnum.Administrator));
    }

    public static Guid? GetUserId(this ClaimsPrincipal? principal)
    {
        var userIdClaim = principal?.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;
        return userIdClaim != null ? Guid.Parse(userIdClaim) : null;
    }

    public static Guid? GetGuid(this ClaimsPrincipal? principal, string claimType)
    {
        var claim = principal?.FindFirst(claimType)?.Value;
        return claim != null ? Guid.Parse(claim) : null;
    }

    public static string? GetUserEmail(this ClaimsPrincipal? principal)
    {
        return principal?.FindFirst(JwtRegisteredClaimNames.Email)?.Value;
    }

    public static string? GetUsername(this ClaimsPrincipal? principal)
    {
        return principal?.FindFirst("unique_name")?.Value;
    }

    public static string? GetRole(this ClaimsPrincipal? principal)
    {
        return principal?.FindFirst("role")?.Value;
    }
    
    public static List<string>? GetRoles(this ClaimsPrincipal? principal)
    {
        return principal?.FindAll("role").Select(r => r.Value).ToList();
    }

    public static string? GetAccessToken(this HttpContext httpContext)
    {
        var authHeader = httpContext.Request.Headers.Authorization.FirstOrDefault();
        if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer "))
        {
            return null;
        }

        return authHeader["Bearer ".Length..];
    }

    public static string? GetFullName(this ClaimsPrincipal? principal)
    {
        return principal?.FindFirst("full_name")?.Value;
    }
    
    public static string? GetStudentId(this ClaimsPrincipal? principal)
    {
        return principal?.FindFirst("student_id")?.Value
               ?? principal?.FindFirst("studentId")?.Value;
    }
    
    public static string? GetTutorId(this ClaimsPrincipal? principal)
    {
        return principal?.FindFirst("tutor_id")?.Value
               ?? principal?.FindFirst("tutorId")?.Value;
    }
    

    public static string? GetPhoneNumber(this ClaimsPrincipal? principal)
    {
        return principal?.FindFirst(ClaimTypes.MobilePhone)?.Value;
    }

    public static ClaimsValidatorBuilder ValidateClaims(this ClaimsPrincipal? principal)
    {
        return new ClaimsValidatorBuilder(principal);
    }

    /**
     * This method provides audit information based on claims
     */
    public static AuditInfo GetAuditInfo(this ClaimsPrincipal? principal)
    {
        return new AuditInfo
        {
            CreatedById = GetUserId(principal),
            CreatedBy = GetUsername(principal),
            Role = GetRole(principal) == null ? null : Enum.Parse<UserRoleEnum>(GetRole(principal)!)
        };
    }
}
