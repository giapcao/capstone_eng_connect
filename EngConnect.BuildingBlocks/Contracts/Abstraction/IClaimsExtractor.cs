using System.Security.Claims;

namespace EngConnect.BuildingBlock.Contracts.Abstraction;

public interface IClaimsExtractor
{
    Guid? ExtractUserId(ClaimsPrincipal principal);
    string? ExtractEmail(ClaimsPrincipal principal);
    string? ExtractUsername(ClaimsPrincipal principal);
    string? ExtractRole(ClaimsPrincipal principal);
    bool HasClaim(ClaimsPrincipal principal, string type, string value);
}