using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.Authentication.RefreshToken;

public class RefreshTokenValidatorTests
{
    [Theory]
    [MemberData(nameof(RefreshTokenTestData.NormalValidatorCases), MemberType = typeof(RefreshTokenTestData))]
    public async Task ValidateAsync_covers_normal_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertValidatorCaseAsync(RefreshTokenTestData.Definition, caseSet);
    }
}