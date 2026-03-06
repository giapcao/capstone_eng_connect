using System.Security.Claims;
using System.Security.Cryptography;
using EngConnect.Domain.Persistence.Models;

namespace EngConnect.Domain.Abstraction;

public interface IJwtTokenService
{
    /// <summary>
    /// Generate access token for user (admin)
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    string GenerateAccessToken(User user);
    
    // /// <summary>
    // /// Generate access token for user (admin)
    // /// </summary>
    // /// <param name="customer"></param>
    // /// <returns></returns>
    // string GenerateAccessToken(Customer customer);

    /// <summary>
    /// Generate refresh token for customer
    /// </summary>
    string GenerateRefreshToken();

    // Validate tokens
    ClaimsPrincipal? ValidateAccessToken(string token, bool validateLifetime = true);

    // Get token information
    IEnumerable<Claim> GetClaimsFromToken(string token);
    DateTime? GetExpirationTimeFromToken(string token);

    // Key management
    RSA GetRsaPublicKey();
}