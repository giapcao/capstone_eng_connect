using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using EngConnect.BuildingBlock.Contracts.Abstraction;

namespace EngConnect.BuildingBlock.Infrastructure.JWT;

public class ClaimsExtractor : IClaimsExtractor
{
    public Guid? ExtractUserId(ClaimsPrincipal principal)
    {
        var claims = principal.Claims;
        var userIdClaim = claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub)?.Value;
        if (userIdClaim == null) return null;
        if (Guid.TryParse(userIdClaim, out var userId)) return userId;
        return null;
    }

    public string? ExtractEmail(ClaimsPrincipal principal)
    {
        return principal.FindFirst(JwtRegisteredClaimNames.Email)?.Value;
    }

    public string? ExtractUsername(ClaimsPrincipal principal)
    {
        return principal.FindFirst(ClaimTypes.Name)?.Value;
    }

    public string? ExtractRole(ClaimsPrincipal principal)
    {
        return principal.FindFirst(ClaimTypes.Role)?.Value;
    }

    public bool HasClaim(ClaimsPrincipal principal, string type, string value)
    {
        return principal.HasClaim(type, value);
    }
}