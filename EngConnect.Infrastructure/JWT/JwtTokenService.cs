using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using EngConnect.BuildingBlock.Contracts.Settings;
using EngConnect.BuildingBlock.Contracts.Shared.Utils;
using EngConnect.BuildingBlock.Domain.Constants;
using EngConnect.Domain.Abstraction;
using EngConnect.Domain.Persistence.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace EngConnect.Infrastructure.JWT;

public class JwtTokenService : IJwtTokenService
{
    private readonly JwtSettings _jwtSettings;
    private readonly RSA _privateRsa;
    private readonly RSA _publicRsa;
    private readonly JwtSecurityTokenHandler _tokenHandler;

    public JwtTokenService(IOptions<JwtSettings> jwtSettings)
    {
        _jwtSettings = jwtSettings.Value;
        _tokenHandler = new JwtSecurityTokenHandler();

        // Initialize RSA for access tokens with validation
        if (!string.IsNullOrEmpty(_jwtSettings.PrivateKeyBytes))
        {
            _privateRsa = _jwtSettings.PrivateKeyBytes.ReadRsaKeyBase64();
        }
        else
        {
            throw new Exception("Private key is not configured. Please check your JwtSettings.");
        }

        if (!string.IsNullOrEmpty(_jwtSettings.PublicKeyBytes))
        {
            _publicRsa = _jwtSettings.PublicKeyBytes.ReadRsaKeyBase64();
        }
        else
        {
            throw new Exception("Public key is not configured. Please check your JwtSettings.");
        }
    }

    public string GenerateAccessToken(User user)
    {
        var claims = BuildUserClaims(user);
        return GenerateAccessTokenFromClaims(claims);
    }
    //
    // public string GenerateAccessToken(Customer customer)
    // {
    //     var claims = BuildCustomerClaims(customer);
    //     return GenerateAccessTokenFromClaims(claims);
    // }

    private string GenerateAccessTokenFromClaims(IEnumerable<Claim> claims)
    {
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(_jwtSettings.AccessTokenExpirationMinutes),
            Issuer = _jwtSettings.Issuer,
            Audience = _jwtSettings.Audience,
            SigningCredentials = new SigningCredentials(new RsaSecurityKey(_privateRsa), SecurityAlgorithms.RsaSha256)
        };

        var token = _tokenHandler.CreateToken(tokenDescriptor);
        return _tokenHandler.WriteToken(token);
    }

    public string GenerateRefreshToken()
    {
        var randomNumber = new byte[64];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }

    public ClaimsPrincipal? ValidateAccessToken(string token, bool validateLifetime = true)
    {
        try
        {
            var principal = _tokenHandler.ValidateToken(token, GetTokenValidationParameters(validateLifetime), out _);
            return principal;
        }
        catch
        {
            return null;
        }
    }

    public IEnumerable<Claim> GetClaimsFromToken(string token)
    {
        try
        {
            var jwtToken = _tokenHandler.ReadJwtToken(token);
            return jwtToken.Claims;
        }
        catch
        {
            return [];
        }
    }

    public DateTime? GetExpirationTimeFromToken(string token)
    {
        try
        {
            var jwtToken = _tokenHandler.ReadJwtToken(token);
            var expClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Exp);

            if (expClaim == null)
                return null;

            // The exp claim is stored as a Unix timestamp (seconds since epoch)
            var unixTime = long.Parse(expClaim.Value);
            return DateTimeOffset.FromUnixTimeSeconds(unixTime).DateTime;
        }
        catch
        {
            return null;
        }
    }

    public RSA GetRsaPublicKey()
    {
        return _publicRsa;
    }

    private TokenValidationParameters GetTokenValidationParameters(bool validateLifetime = true)
    {
        return new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new RsaSecurityKey(_publicRsa),
            ValidateIssuer = true,
            ValidIssuer = _jwtSettings.Issuer,
            ValidateAudience = true,
            ValidAudience = _jwtSettings.Audience,
            ValidateLifetime = validateLifetime,
            ClockSkew = TimeSpan.Zero
        };
    }

    // Helper method to build claims from User entity
    private static List<Claim> BuildUserClaims(User user)
    {
        var roles = user!.UserRoles
            .Select(ur => ur.Role.Code)
            .ToList();
        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim("firstname", user.FirstName),
            new Claim("lastname", user.LastName),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };
        
        //Add role claims 
        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }
        return claims;
    }
    
    // // Helper method to build claims from Customer entity
    // private static List<Claim> BuildCustomerClaims(Customer user)
    // {
    //     return
    //     [
    //         new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
    //         new Claim(JwtRegisteredClaimNames.Email, user.Email),
    //         new Claim(ClaimTypes.Role, nameof(UserRole.Customer)),
    //         new Claim("firstname", user.Firstname),
    //         new Claim("lastname", user.Lastname),
    //         new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
    //     ];
    // }
}