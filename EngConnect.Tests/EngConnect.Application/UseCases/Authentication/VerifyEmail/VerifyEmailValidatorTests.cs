using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.Authentication.VerifyEmail;

public class VerifyEmailValidatorTests
{
    [Theory]
    [MemberData(nameof(VerifyEmailTestData.NormalValidatorCases), MemberType = typeof(VerifyEmailTestData))]
    public async Task ValidateAsync_covers_normal_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertValidatorCaseAsync(VerifyEmailTestData.Definition, caseSet);
    }
}