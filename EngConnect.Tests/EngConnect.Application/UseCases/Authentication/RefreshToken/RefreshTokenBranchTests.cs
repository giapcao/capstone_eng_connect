using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.Authentication.RefreshToken;

public class RefreshTokenBranchTests
{
    [Theory]
    [MemberData(nameof(RefreshTokenTestData.HandlerBranchCases), MemberType = typeof(RefreshTokenTestData))]
    public async Task HandleAsync_covers_branch_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertHandlerCaseAsync(RefreshTokenTestData.Definition, caseSet);
    }

    [Theory]
    [MemberData(nameof(RefreshTokenTestData.ValidatorBranchCases), MemberType = typeof(RefreshTokenTestData))]
    public async Task ValidateAsync_covers_branch_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertValidatorCaseAsync(RefreshTokenTestData.Definition, caseSet);
    }
}