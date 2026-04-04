using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using EngConnect.BuildingBlock.Contracts.Settings;
using EngConnect.Domain.Persistence.Models;
using EngConnect.Infrastructure.JWT;
using EngConnect.Tests.Common;
using Microsoft.Extensions.Options;
using Xunit;

namespace EngConnect.Tests.EngConnect.Infrastructure.JWT;

public class JwtTokenServiceTests
{
    [Fact]
    public void GenerateAccessToken_creates_token_with_expected_claims()
    {
        var settings = TestSettingsFactory.Create<JwtSettings>();
        var sut = new JwtTokenService(Options.Create(settings));
        var userId = Guid.NewGuid();
        var tutorId = Guid.NewGuid();
        var studentId = Guid.NewGuid();

        var user = new User
        {
            Id = userId,
            FirstName = "Test",
            LastName = "User",
            UserName = "tester",
            Email = "tester@example.com",
            PasswordHash = "hash",
            Status = "Active",
            Tutor = new Tutor { Id = tutorId, UserId = userId },
            Student = new Student { Id = studentId, UserId = userId },
            UserRoles =
            [
                new UserRole
                {
                    Role = new Role { Code = "Tutor" }
                }
            ]
        };

        var token = sut.GenerateAccessToken(user);
        var principal = sut.ValidateAccessToken(token);
        var claims = sut.GetClaimsFromToken(token).ToList();

        Assert.NotNull(principal);
        Assert.Contains(principal!.FindAll(ClaimTypes.Role), claim => claim.Value == "Tutor");
        Assert.Contains(claims, claim => claim.Type == JwtRegisteredClaimNames.Sub && claim.Value == userId.ToString());
        Assert.Contains(claims, claim => claim.Type == JwtRegisteredClaimNames.Email && claim.Value == user.Email);
        Assert.Contains(claims, claim => claim.Type == "tutorId" && claim.Value == tutorId.ToString());
        Assert.Contains(claims, claim => claim.Type == "studentId" && claim.Value == studentId.ToString());
        Assert.True(sut.GetExpirationTimeFromToken(token) > DateTime.UtcNow);
    }

    [Fact]
    public void Invalid_tokens_return_empty_claims_and_null_principal()
    {
        var settings = TestSettingsFactory.Create<JwtSettings>();
        var sut = new JwtTokenService(Options.Create(settings));

        var principal = sut.ValidateAccessToken("invalid-token");
        var claims = sut.GetClaimsFromToken("invalid-token");

        Assert.Null(principal);
        Assert.Empty(claims);
        Assert.Null(sut.GetExpirationTimeFromToken("invalid-token"));
    }
}
